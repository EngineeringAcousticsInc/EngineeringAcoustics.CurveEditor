#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2020 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */

/* Derived from code by the Mono.Xna Team (Copyright 2006).
 * Released under the MIT License. See monoxna.LICENSE for details.
 */
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using System.Text;
#endregion

namespace Microsoft.Xna.Framework
{
	/// <summary>
	/// Defines a viewing frustum for intersection operations.
	/// </summary>
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	public class BoundingFrustum : IEquatable<BoundingFrustum>
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the <see cref="Matrix"/> of the frustum.
		/// </summary>
		public Matrix Matrix
		{
			get => _matrix;
			set
			{
				/* FIXME: The odds are the planes will be used a lot more often than
				 * the matrix is updated, so this should help performance. I hope. ;)
				 */
				_matrix = value;
				CreatePlanes();
				CreateCorners();
			}
		}

		/// <summary>
		/// Gets the near plane of the frustum.
		/// </summary>
		public Plane Near => _planes[0];

		/// <summary>
		/// Gets the far plane of the frustum.
		/// </summary>
		public Plane Far => _planes[1];

		/// <summary>
		/// Gets the left plane of the frustum.
		/// </summary>
		public Plane Left => _planes[2];

		/// <summary>
		/// Gets the right plane of the frustum.
		/// </summary>
		public Plane Right => _planes[3];

		/// <summary>
		/// Gets the top plane of the frustum.
		/// </summary>
		public Plane Top => _planes[4];

		/// <summary>
		/// Gets the bottom plane of the frustum.
		/// </summary>
		public Plane Bottom => _planes[5];

		#endregion

		#region Internal Properties

		internal string DebugDisplayString => string.Concat(
					"Near( ", _planes[0].DebugDisplayString, " ) \r\n",
					"Far( ", _planes[1].DebugDisplayString, " ) \r\n",
					"Left( ", _planes[2].DebugDisplayString, " ) \r\n",
					"Right( ", _planes[3].DebugDisplayString, " ) \r\n",
					"Top( ", _planes[4].DebugDisplayString, " ) \r\n",
					"Bottom( ", _planes[5].DebugDisplayString, " ) "
				);

		#endregion

		#region Public Fields

		/// <summary>
		/// The number of corner points in the frustum.
		/// </summary>
		public const int CornerCount = 8;

		#endregion

		#region Private Fields

		private Matrix _matrix;
		private readonly Vector3[] _corners = new Vector3[CornerCount];
		private readonly Plane[] _planes = new Plane[PlaneCount];

		/// <summary>
		/// The number of planes in the frustum.
		/// </summary>
		private const int PlaneCount = 6;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Constructs the frustum by extracting the view planes from a matrix.
		/// </summary>
		/// <param name="value">Combined matrix which usually is (View * Projection).</param>
		public BoundingFrustum(Matrix value)
		{
			_matrix = value;
			CreatePlanes();
			CreateCorners();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Containment test between this <see cref="BoundingFrustum"/> and specified <see cref="BoundingFrustum"/>.
		/// </summary>
		/// <param name="frustum">A <see cref="BoundingFrustum"/> for testing.</param>
		/// <returns>Result of testing for containment between this <see cref="BoundingFrustum"/> and specified <see cref="BoundingFrustum"/>.</returns>
		public ContainmentType Contains(BoundingFrustum frustum)
		{
			if (this == frustum)
			{
				return ContainmentType.Contains;
			}
			bool intersects = false;
			for (int i = 0; i < PlaneCount; i += 1)
			{
				frustum.Intersects(ref _planes[i], out PlaneIntersectionType planeIntersectionType);
				if (planeIntersectionType == PlaneIntersectionType.Front)
				{
					return ContainmentType.Disjoint;
				}
				else if (planeIntersectionType == PlaneIntersectionType.Intersecting)
				{
					intersects = true;
				}
			}
			return intersects ? ContainmentType.Intersects : ContainmentType.Contains;
		}

		/// <summary>
		/// Containment test between this <see cref="BoundingFrustum"/> and specified <see cref="BoundingBox"/>.
		/// </summary>
		/// <param name="box">A <see cref="BoundingBox"/> for testing.</param>
		/// <returns>Result of testing for containment between this <see cref="BoundingFrustum"/> and specified <see cref="BoundingBox"/>.</returns>
		public ContainmentType Contains(BoundingBox box)
		{
			Contains(ref box, out ContainmentType result);
			return result;
		}

		/// <summary>
		/// Containment test between this <see cref="BoundingFrustum"/> and specified <see cref="BoundingBox"/>.
		/// </summary>
		/// <param name="box">A <see cref="BoundingBox"/> for testing.</param>
		/// <param name="result">Result of testing for containment between this <see cref="BoundingFrustum"/> and specified <see cref="BoundingBox"/> as an output parameter.</param>
		public void Contains(ref BoundingBox box, out ContainmentType result)
		{
			bool intersects = false;
			for (int i = 0; i < PlaneCount; i += 1)
			{
				box.Intersects(ref _planes[i], out PlaneIntersectionType planeIntersectionType);
				switch (planeIntersectionType)
				{
					case PlaneIntersectionType.Front:
						result = ContainmentType.Disjoint;
						return;
					case PlaneIntersectionType.Intersecting:
						intersects = true;
						break;
				}
			}
			result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
		}

		/// <summary>
		/// Containment test between this <see cref="BoundingFrustum"/> and specified <see cref="BoundingSphere"/>.
		/// </summary>
		/// <param name="sphere">A <see cref="BoundingSphere"/> for testing.</param>
		/// <returns>Result of testing for containment between this <see cref="BoundingFrustum"/> and specified <see cref="BoundingSphere"/>.</returns>
		public ContainmentType Contains(BoundingSphere sphere)
		{
			Contains(ref sphere, out ContainmentType result);
			return result;
		}

		/// <summary>
		/// Containment test between this <see cref="BoundingFrustum"/> and specified <see cref="BoundingSphere"/>.
		/// </summary>
		/// <param name="sphere">A <see cref="BoundingSphere"/> for testing.</param>
		/// <param name="result">Result of testing for containment between this <see cref="BoundingFrustum"/> and specified <see cref="BoundingSphere"/> as an output parameter.</param>
		public void Contains(ref BoundingSphere sphere, out ContainmentType result)
		{
			bool intersects = false;
			for (int i = 0; i < PlaneCount; i += 1)
			{

				// TODO: We might want to inline this for performance reasons.
				sphere.Intersects(ref _planes[i], out PlaneIntersectionType planeIntersectionType);
				switch (planeIntersectionType)
				{
					case PlaneIntersectionType.Front:
						result = ContainmentType.Disjoint;
						return;
					case PlaneIntersectionType.Intersecting:
						intersects = true;
						break;
				}
			}
			result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
		}

		/// <summary>
		/// Containment test between this <see cref="BoundingFrustum"/> and specified <see cref="Vector3"/>.
		/// </summary>
		/// <param name="point">A <see cref="Vector3"/> for testing.</param>
		/// <returns>Result of testing for containment between this <see cref="BoundingFrustum"/> and specified <see cref="Vector3"/>.</returns>
		public ContainmentType Contains(Vector3 point)
		{
			Contains(ref point, out ContainmentType result);
			return result;
		}

		/// <summary>
		/// Containment test between this <see cref="BoundingFrustum"/> and specified <see cref="Vector3"/>.
		/// </summary>
		/// <param name="point">A <see cref="Vector3"/> for testing.</param>
		/// <param name="result">Result of testing for containment between this <see cref="BoundingFrustum"/> and specified <see cref="Vector3"/> as an output parameter.</param>
		public void Contains(ref Vector3 point, out ContainmentType result)
		{
			bool intersects = false;
			for (int i = 0; i < PlaneCount; i += 1)
			{
				float classifyPoint =
					(point.X * _planes[i].Normal.X) +
					(point.Y * _planes[i].Normal.Y) +
					(point.Z * _planes[i].Normal.Z) +
					_planes[i].D
				;
				if (classifyPoint > 0)
				{
					result = ContainmentType.Disjoint;
					return;
				}
				else if (classifyPoint == 0)
				{
					intersects = true;
					break;
				}
			}
			result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
		}

		/// <summary>
		/// Returns a copy of internal corners array.
		/// </summary>
		/// <returns>The array of corners.</returns>
		public Vector3[] GetCorners() => (Vector3[])_corners.Clone();

		/// <summary>
		/// Returns a copy of internal corners array.
		/// </summary>
		/// <param name="corners">The array which values will be replaced to corner values of this instance. It must have size of <see cref="BoundingFrustum.CornerCount"/>.</param>
		public void GetCorners(Vector3[] corners)
		{
			if (corners == null)
			{
				throw new ArgumentNullException("corners");
			}
			if (corners.Length < CornerCount)
			{
				throw new ArgumentOutOfRangeException("corners");
			}

			_corners.CopyTo(corners, 0);
		}

		/// <summary>
		/// Gets whether or not a specified <see cref="BoundingFrustum"/> intersects with this <see cref="BoundingFrustum"/>.
		/// </summary>
		/// <param name="frustum">An other <see cref="BoundingFrustum"/> for intersection test.</param>
		/// <returns><c>true</c> if other <see cref="BoundingFrustum"/> intersects with this <see cref="BoundingFrustum"/>; <c>false</c> otherwise.</returns>
		public bool Intersects(BoundingFrustum frustum) => Contains(frustum) != ContainmentType.Disjoint;

		/// <summary>
		/// Gets whether or not a specified <see cref="BoundingBox"/> intersects with this <see cref="BoundingFrustum"/>.
		/// </summary>
		/// <param name="box">A <see cref="BoundingBox"/> for intersection test.</param>
		/// <returns><c>true</c> if specified <see cref="BoundingBox"/> intersects with this <see cref="BoundingFrustum"/>; <c>false</c> otherwise.</returns>
		public bool Intersects(BoundingBox box)
		{
			Intersects(ref box, out bool result);
			return result;
		}

		/// <summary>
		/// Gets whether or not a specified <see cref="BoundingBox"/> intersects with this <see cref="BoundingFrustum"/>.
		/// </summary>
		/// <param name="box">A <see cref="BoundingBox"/> for intersection test.</param>
		/// <param name="result"><c>true</c> if specified <see cref="BoundingBox"/> intersects with this <see cref="BoundingFrustum"/>; <c>false</c> otherwise as an output parameter.</param>
		public void Intersects(ref BoundingBox box, out bool result)
		{
			Contains(ref box, out ContainmentType containment);
			result = containment != ContainmentType.Disjoint;
		}

		/// <summary>
		/// Gets whether or not a specified <see cref="BoundingSphere"/> intersects with this <see cref="BoundingFrustum"/>.
		/// </summary>
		/// <param name="sphere">A <see cref="BoundingSphere"/> for intersection test.</param>
		/// <returns><c>true</c> if specified <see cref="BoundingSphere"/> intersects with this <see cref="BoundingFrustum"/>; <c>false</c> otherwise.</returns>
		public bool Intersects(BoundingSphere sphere)
		{
			Intersects(ref sphere, out bool result);
			return result;
		}

		/// <summary>
		/// Gets whether or not a specified <see cref="BoundingSphere"/> intersects with this <see cref="BoundingFrustum"/>.
		/// </summary>
		/// <param name="sphere">A <see cref="BoundingSphere"/> for intersection test.</param>
		/// <param name="result"><c>true</c> if specified <see cref="BoundingSphere"/> intersects with this <see cref="BoundingFrustum"/>; <c>false</c> otherwise as an output parameter.</param>
		public void Intersects(ref BoundingSphere sphere, out bool result)
		{
			Contains(ref sphere, out ContainmentType containment);
			result = containment != ContainmentType.Disjoint;
		}

		/// <summary>
		/// Gets type of intersection between specified <see cref="Plane"/> and this <see cref="BoundingFrustum"/>.
		/// </summary>
		/// <param name="plane">A <see cref="Plane"/> for intersection test.</param>
		/// <returns>A plane intersection type.</returns>
		public PlaneIntersectionType Intersects(Plane plane)
		{
			Intersects(ref plane, out PlaneIntersectionType result);
			return result;
		}

		/// <summary>
		/// Gets type of intersection between specified <see cref="Plane"/> and this <see cref="BoundingFrustum"/>.
		/// </summary>
		/// <param name="plane">A <see cref="Plane"/> for intersection test.</param>
		/// <param name="result">A plane intersection type as an output parameter.</param>
		public void Intersects(ref Plane plane, out PlaneIntersectionType result)
		{
			result = plane.Intersects(ref _corners[0]);
			for (int i = 1; i < _corners.Length; i += 1)
			{
				if (plane.Intersects(ref _corners[i]) != result)
				{
					result = PlaneIntersectionType.Intersecting;
				}
			}
		}

		/// <summary>
		/// Gets the distance of intersection of <see cref="Ray"/> and this <see cref="BoundingFrustum"/> or null if no intersection happens.
		/// </summary>
		/// <param name="ray">A <see cref="Ray"/> for intersection test.</param>
		/// <returns>Distance at which ray intersects with this <see cref="BoundingFrustum"/> or null if no intersection happens.</returns>
		public float? Intersects(Ray ray)
		{
			Intersects(ref ray, out float? result);
			return result;
		}

		/// <summary>
		/// Gets the distance of intersection of <see cref="Ray"/> and this <see cref="BoundingFrustum"/> or null if no intersection happens.
		/// </summary>
		/// <param name="ray">A <see cref="Ray"/> for intersection test.</param>
		/// <param name="result">Distance at which ray intersects with this <see cref="BoundingFrustum"/> or null if no intersection happens as an output parameter.</param>
		public void Intersects(ref Ray ray, out float? result)
		{
			Contains(ref ray.Position, out ContainmentType ctype);

			if (ctype == ContainmentType.Disjoint)
			{
				result = null;
				return;
			}
			if (ctype == ContainmentType.Contains)
			{
				result = 0.0f;
				return;
			}
			if (ctype != ContainmentType.Intersects)
			{
				throw new ArgumentOutOfRangeException("ctype");
			}

			throw new NotImplementedException();
		}

		#endregion

		#region Private Methods

		private void CreateCorners()
		{
			IntersectionPoint(
				ref _planes[0],
				ref _planes[2],
				ref _planes[4],
				out _corners[0]
			);
			IntersectionPoint(
				ref _planes[0],
				ref _planes[3],
				ref _planes[4],
				out _corners[1]
			);
			IntersectionPoint(
				ref _planes[0],
				ref _planes[3],
				ref _planes[5],
				out _corners[2]
			);
			IntersectionPoint(
				ref _planes[0],
				ref _planes[2],
				ref _planes[5],
				out _corners[3]
			);
			IntersectionPoint(
				ref _planes[1],
				ref _planes[2],
				ref _planes[4],
				out _corners[4]
			);
			IntersectionPoint(
				ref _planes[1],
				ref _planes[3],
				ref _planes[4],
				out _corners[5]
			);
			IntersectionPoint(
				ref _planes[1],
				ref _planes[3],
				ref _planes[5],
				out _corners[6]
			);
			IntersectionPoint(
				ref _planes[1],
				ref _planes[2],
				ref _planes[5],
				out _corners[7]
			);
		}

		private void CreatePlanes()
		{
			_planes[0] = new Plane(
				-_matrix.M13,
				-_matrix.M23,
				-_matrix.M33,
				-_matrix.M43
			);
			_planes[1] = new Plane(
				_matrix.M13 - _matrix.M14,
				_matrix.M23 - _matrix.M24,
				_matrix.M33 - _matrix.M34,
				_matrix.M43 - _matrix.M44
			);
			_planes[2] = new Plane(
				-_matrix.M14 - _matrix.M11,
				-_matrix.M24 - _matrix.M21,
				-_matrix.M34 - _matrix.M31,
				-_matrix.M44 - _matrix.M41
			);
			_planes[3] = new Plane(
				_matrix.M11 - _matrix.M14,
				_matrix.M21 - _matrix.M24,
				_matrix.M31 - _matrix.M34,
				_matrix.M41 - _matrix.M44
			);
			_planes[4] = new Plane(
				_matrix.M12 - _matrix.M14,
				_matrix.M22 - _matrix.M24,
				_matrix.M32 - _matrix.M34,
				_matrix.M42 - _matrix.M44
			);
			_planes[5] = new Plane(
				-_matrix.M14 - _matrix.M12,
				-_matrix.M24 - _matrix.M22,
				-_matrix.M34 - _matrix.M32,
				-_matrix.M44 - _matrix.M42
			);

			NormalizePlane(ref _planes[0]);
			NormalizePlane(ref _planes[1]);
			NormalizePlane(ref _planes[2]);
			NormalizePlane(ref _planes[3]);
			NormalizePlane(ref _planes[4]);
			NormalizePlane(ref _planes[5]);
		}

		private void NormalizePlane(ref Plane p)
		{
			float factor = 1f / p.Normal.Length();
			p.Normal.X *= factor;
			p.Normal.Y *= factor;
			p.Normal.Z *= factor;
			p.D *= factor;
		}

		#endregion

		#region Private Static Methods

		private static void IntersectionPoint(
			ref Plane a,
			ref Plane b,
			ref Plane c,
			out Vector3 result
		)
		{
			/* Formula used
			 *                d1 ( N2 * N3 ) + d2 ( N3 * N1 ) + d3 ( N1 * N2 )
			 * P =   -------------------------------------------------------------------
			 *                             N1 . ( N2 * N3 )
			 *
			 * Note: N refers to the normal, d refers to the displacement. '.' means dot
			 * product. '*' means cross product
			 */


			Vector3.Cross(ref b.Normal, ref c.Normal, out Vector3 cross);

			Vector3.Dot(ref a.Normal, ref cross, out float f);
			f *= -1.0f;

			Vector3.Cross(ref b.Normal, ref c.Normal, out cross);
			Vector3.Multiply(ref cross, a.D, out Vector3 v1);
			// v1 = (a.D * (Vector3.Cross(b.Normal, c.Normal)));


			Vector3.Cross(ref c.Normal, ref a.Normal, out cross);
			Vector3.Multiply(ref cross, b.D, out Vector3 v2);
			// v2 = (b.D * (Vector3.Cross(c.Normal, a.Normal)));


			Vector3.Cross(ref a.Normal, ref b.Normal, out cross);
			Vector3.Multiply(ref cross, c.D, out Vector3 v3);
			// v3 = (c.D * (Vector3.Cross(a.Normal, b.Normal)));

			result.X = (v1.X + v2.X + v3.X) / f;
			result.Y = (v1.Y + v2.Y + v3.Y) / f;
			result.Z = (v1.Z + v2.Z + v3.Z) / f;
		}

		#endregion

		#region Public Static Operators and Override Methods

		/// <summary>
		/// Compares whether two <see cref="BoundingFrustum"/> instances are equal.
		/// </summary>
		/// <param name="a"><see cref="BoundingFrustum"/> instance on the left of the equal sign.</param>
		/// <param name="b"><see cref="BoundingFrustum"/> instance on the right of the equal sign.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public static bool operator ==(BoundingFrustum a, BoundingFrustum b)
		{
			if (object.Equals(a, null))
			{
				return object.Equals(b, null);
			}

			if (object.Equals(b, null))
			{
				return object.Equals(a, null);
			}

			return a._matrix == b._matrix;
		}

		/// <summary>
		/// Compares whether two <see cref="BoundingFrustum"/> instances are not equal.
		/// </summary>
		/// <param name="a"><see cref="BoundingFrustum"/> instance on the left of the not equal sign.</param>
		/// <param name="b"><see cref="BoundingFrustum"/> instance on the right of the not equal sign.</param>
		/// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
		public static bool operator !=(BoundingFrustum a, BoundingFrustum b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Compares whether current instance is equal to specified <see cref="BoundingFrustum"/>.
		/// </summary>
		/// <param name="other">The <see cref="BoundingFrustum"/> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public bool Equals(BoundingFrustum other) => this == other;

		/// <summary>
		/// Compares whether current instance is equal to specified <see cref="BoundingFrustum"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public override bool Equals(object obj) => (obj is BoundingFrustum frustum) && Equals(frustum);

		/// <summary>
		/// Returns a <see cref="string"/> representation of this <see cref="BoundingFrustum"/> in the format:
		/// {Near:[nearPlane] Far:[farPlane] Left:[leftPlane] Right:[rightPlane] Top:[topPlane] Bottom:[bottomPlane]}
		/// </summary>
		/// <returns><see cref="string"/> representation of this <see cref="BoundingFrustum"/>.</returns>
		public override string ToString()
		{
			var sb = new StringBuilder(256);
			_ = sb.Append("{Near:");
			_ = sb.Append(_planes[0].ToString());
			_ = sb.Append(" Far:");
			_ = sb.Append(_planes[1].ToString());
			_ = sb.Append(" Left:");
			_ = sb.Append(_planes[2].ToString());
			_ = sb.Append(" Right:");
			_ = sb.Append(_planes[3].ToString());
			_ = sb.Append(" Top:");
			_ = sb.Append(_planes[4].ToString());
			_ = sb.Append(" Bottom:");
			_ = sb.Append(_planes[5].ToString());
			_ = sb.Append("}");
			return sb.ToString();
		}

		/// <summary>
		/// Gets the hash code of this <see cref="BoundingFrustum"/>.
		/// </summary>
		/// <returns>Hash code of this <see cref="BoundingFrustum"/>.</returns>
		public override int GetHashCode() => _matrix.GetHashCode();

		#endregion
	}
}

