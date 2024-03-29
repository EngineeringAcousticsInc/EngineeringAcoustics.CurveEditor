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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using Microsoft.Xna.Framework.Design;
#endregion

namespace Microsoft.Xna.Framework
{
	[Serializable]
	[TypeConverter(typeof(BoundingBoxConverter))]
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	public struct BoundingBox : IEquatable<BoundingBox>
	{
		#region Internal Properties

		internal string DebugDisplayString => string.Concat(
					"Min( ", Min.DebugDisplayString, " ) \r\n",
					"Max( ", Max.DebugDisplayString, " )"
				);

		#endregion

		#region Public Fields

		public Vector3 Min;

		public Vector3 Max;

		public const int CornerCount = 8;

		#endregion

		#region Private Static Variables

		private static readonly Vector3 MaxVector3 = new Vector3(float.MaxValue);
		private static readonly Vector3 MinVector3 = new Vector3(float.MinValue);

		#endregion

		#region Public Constructors

		public BoundingBox(Vector3 min, Vector3 max)
		{
			Min = min;
			Max = max;
		}

		#endregion

		#region Public Methods

		public void Contains(ref BoundingBox box, out ContainmentType result) => result = Contains(box);

		public void Contains(ref BoundingSphere sphere, out ContainmentType result) => result = Contains(sphere);

		public ContainmentType Contains(Vector3 point)
		{
			Contains(ref point, out ContainmentType result);
			return result;
		}

		public ContainmentType Contains(BoundingBox box)
		{
			// Test if all corner is in the same side of a face by just checking min and max
			if (box.Max.X < Min.X ||
				box.Min.X > Max.X ||
				box.Max.Y < Min.Y ||
				box.Min.Y > Max.Y ||
				box.Max.Z < Min.Z ||
				box.Min.Z > Max.Z)
			{
				return ContainmentType.Disjoint;
			}


			if (box.Min.X >= Min.X &&
				box.Max.X <= Max.X &&
				box.Min.Y >= Min.Y &&
				box.Max.Y <= Max.Y &&
				box.Min.Z >= Min.Z &&
				box.Max.Z <= Max.Z)
			{
				return ContainmentType.Contains;
			}

			return ContainmentType.Intersects;
		}

		public ContainmentType Contains(BoundingFrustum frustum)
		{
			/* TODO: bad done here need a fix.
			 * Because the question is not if frustum contains box but the reverse and
			 * this is not the same.
			 */
			int i;
			ContainmentType contained;
			Vector3[] corners = frustum.GetCorners();

			// First we check if frustum is in box.
			for (i = 0; i < corners.Length; i += 1)
			{
				Contains(ref corners[i], out contained);
				if (contained == ContainmentType.Disjoint)
				{
					break;
				}
			}

			// This means we checked all the corners and they were all contain or instersect
			if (i == corners.Length)
			{
				return ContainmentType.Contains;
			}

			// If i is not equal to zero, we can fastpath and say that this box intersects
			if (i != 0)
			{
				return ContainmentType.Intersects;
			}


			/* If we get here, it means the first (and only) point we checked was
			 * actually contained in the frustum. So we assume that all other points
			 * will also be contained. If one of the points is disjoint, we can
			 * exit immediately saying that the result is Intersects
			 */
			i += 1;
			for (; i < corners.Length; i += 1)
			{
				Contains(ref corners[i], out contained);
				if (contained != ContainmentType.Contains)
				{
					return ContainmentType.Intersects;
				}

			}

			/* If we get here, then we know all the points were actually contained,
			 * therefore result is Contains.
			 */
			return ContainmentType.Contains;
		}

		public ContainmentType Contains(BoundingSphere sphere)
		{
			if (sphere.Center.X - Min.X >= sphere.Radius &&
				sphere.Center.Y - Min.Y >= sphere.Radius &&
				sphere.Center.Z - Min.Z >= sphere.Radius &&
				Max.X - sphere.Center.X >= sphere.Radius &&
				Max.Y - sphere.Center.Y >= sphere.Radius &&
				Max.Z - sphere.Center.Z >= sphere.Radius)
			{
				return ContainmentType.Contains;
			}

			double dmin = 0;

			double e = sphere.Center.X - Min.X;
			if (e < 0)
			{
				if (e < -sphere.Radius)
				{
					return ContainmentType.Disjoint;
				}
				dmin += e * e;
			}
			else
			{
				e = sphere.Center.X - Max.X;
				if (e > 0)
				{
					if (e > sphere.Radius)
					{
						return ContainmentType.Disjoint;
					}
					dmin += e * e;
				}
			}

			e = sphere.Center.Y - Min.Y;
			if (e < 0)
			{
				if (e < -sphere.Radius)
				{
					return ContainmentType.Disjoint;
				}
				dmin += e * e;
			}
			else
			{
				e = sphere.Center.Y - Max.Y;
				if (e > 0)
				{
					if (e > sphere.Radius)
					{
						return ContainmentType.Disjoint;
					}
					dmin += e * e;
				}
			}

			e = sphere.Center.Z - Min.Z;
			if (e < 0)
			{
				if (e < -sphere.Radius)
				{
					return ContainmentType.Disjoint;
				}
				dmin += e * e;
			}
			else
			{
				e = sphere.Center.Z - Max.Z;
				if (e > 0)
				{
					if (e > sphere.Radius)
					{
						return ContainmentType.Disjoint;
					}
					dmin += e * e;
				}
			}

			if (dmin <= sphere.Radius * sphere.Radius)
			{
				return ContainmentType.Intersects;
			}

			return ContainmentType.Disjoint;
		}

		public void Contains(ref Vector3 point, out ContainmentType result)
		{
			// Determine if point is outside of this box.
			result = point.X < Min.X ||
				point.X > Max.X ||
				point.Y < Min.Y ||
				point.Y > Max.Y ||
				point.Z < Min.Z ||
				point.Z > Max.Z
				? ContainmentType.Disjoint
				: ContainmentType.Contains;
		}

		public Vector3[] GetCorners()
		{
			return new Vector3[] {
				new Vector3(Min.X, Max.Y, Max.Z),
				new Vector3(Max.X, Max.Y, Max.Z),
				new Vector3(Max.X, Min.Y, Max.Z),
				new Vector3(Min.X, Min.Y, Max.Z),
				new Vector3(Min.X, Max.Y, Min.Z),
				new Vector3(Max.X, Max.Y, Min.Z),
				new Vector3(Max.X, Min.Y, Min.Z),
				new Vector3(Min.X, Min.Y, Min.Z)
			};
		}

		public void GetCorners(Vector3[] corners)
		{
			if (corners == null)
			{
				throw new ArgumentNullException("corners");
			}
			if (corners.Length < 8)
			{
				throw new ArgumentOutOfRangeException("corners", "Not Enought Corners");
			}
			corners[0].X = Min.X;
			corners[0].Y = Max.Y;
			corners[0].Z = Max.Z;
			corners[1].X = Max.X;
			corners[1].Y = Max.Y;
			corners[1].Z = Max.Z;
			corners[2].X = Max.X;
			corners[2].Y = Min.Y;
			corners[2].Z = Max.Z;
			corners[3].X = Min.X;
			corners[3].Y = Min.Y;
			corners[3].Z = Max.Z;
			corners[4].X = Min.X;
			corners[4].Y = Max.Y;
			corners[4].Z = Min.Z;
			corners[5].X = Max.X;
			corners[5].Y = Max.Y;
			corners[5].Z = Min.Z;
			corners[6].X = Max.X;
			corners[6].Y = Min.Y;
			corners[6].Z = Min.Z;
			corners[7].X = Min.X;
			corners[7].Y = Min.Y;
			corners[7].Z = Min.Z;
		}

		public Nullable<float> Intersects(Ray ray) => ray.Intersects(this);

		public void Intersects(ref Ray ray, out Nullable<float> result) => result = Intersects(ray);

		public bool Intersects(BoundingFrustum frustum) => frustum.Intersects(this);

		public void Intersects(ref BoundingSphere sphere, out bool result) => result = Intersects(sphere);

		public bool Intersects(BoundingBox box)
		{
			Intersects(ref box, out bool result);
			return result;
		}

		public PlaneIntersectionType Intersects(Plane plane)
		{
			Intersects(ref plane, out PlaneIntersectionType result);
			return result;
		}

		public void Intersects(ref BoundingBox box, out bool result)
		{
			if ((Max.X >= box.Min.X) && (Min.X <= box.Max.X))
			{
				if ((Max.Y < box.Min.Y) || (Min.Y > box.Max.Y))
				{
					result = false;
					return;
				}

				result = (Max.Z >= box.Min.Z) && (Min.Z <= box.Max.Z);
				return;
			}

			result = false;
			return;
		}

		public bool Intersects(BoundingSphere sphere)
		{
			if (sphere.Center.X - Min.X > sphere.Radius &&
				sphere.Center.Y - Min.Y > sphere.Radius &&
				sphere.Center.Z - Min.Z > sphere.Radius &&
				Max.X - sphere.Center.X > sphere.Radius &&
				Max.Y - sphere.Center.Y > sphere.Radius &&
				Max.Z - sphere.Center.Z > sphere.Radius)
			{
				return true;
			}

			double dmin = 0;

			if (sphere.Center.X - Min.X <= sphere.Radius)
			{
				dmin += (sphere.Center.X - Min.X) * (sphere.Center.X - Min.X);
			}
			else if (Max.X - sphere.Center.X <= sphere.Radius)
			{
				dmin += (sphere.Center.X - Max.X) * (sphere.Center.X - Max.X);
			}

			if (sphere.Center.Y - Min.Y <= sphere.Radius)
			{
				dmin += (sphere.Center.Y - Min.Y) * (sphere.Center.Y - Min.Y);
			}
			else if (Max.Y - sphere.Center.Y <= sphere.Radius)
			{
				dmin += (sphere.Center.Y - Max.Y) * (sphere.Center.Y - Max.Y);
			}

			if (sphere.Center.Z - Min.Z <= sphere.Radius)
			{
				dmin += (sphere.Center.Z - Min.Z) * (sphere.Center.Z - Min.Z);
			}
			else if (Max.Z - sphere.Center.Z <= sphere.Radius)
			{
				dmin += (sphere.Center.Z - Max.Z) * (sphere.Center.Z - Max.Z);
			}

			if (dmin <= sphere.Radius * sphere.Radius)
			{
				return true;
			}

			return false;
		}

		public void Intersects(ref Plane plane, out PlaneIntersectionType result)
		{
			// See http://zach.in.tu-clausthal.de/teaching/cg_literatur/lighthouse3d_view_frustum_culling/index.html

			Vector3 positiveVertex;
			Vector3 negativeVertex;

			if (plane.Normal.X >= 0)
			{
				positiveVertex.X = Max.X;
				negativeVertex.X = Min.X;
			}
			else
			{
				positiveVertex.X = Min.X;
				negativeVertex.X = Max.X;
			}

			if (plane.Normal.Y >= 0)
			{
				positiveVertex.Y = Max.Y;
				negativeVertex.Y = Min.Y;
			}
			else
			{
				positiveVertex.Y = Min.Y;
				negativeVertex.Y = Max.Y;
			}

			if (plane.Normal.Z >= 0)
			{
				positiveVertex.Z = Max.Z;
				negativeVertex.Z = Min.Z;
			}
			else
			{
				positiveVertex.Z = Min.Z;
				negativeVertex.Z = Max.Z;
			}

			// Inline Vector3.Dot(plane.Normal, negativeVertex) + plane.D;
			float distance =
				(plane.Normal.X * negativeVertex.X) +
				(plane.Normal.Y * negativeVertex.Y) +
				(plane.Normal.Z * negativeVertex.Z) +
				plane.D
			;
			if (distance > 0)
			{
				result = PlaneIntersectionType.Front;
				return;
			}

			// Inline Vector3.Dot(plane.Normal, positiveVertex) + plane.D;
			distance =
				(plane.Normal.X * positiveVertex.X) +
				(plane.Normal.Y * positiveVertex.Y) +
				(plane.Normal.Z * positiveVertex.Z) +
				plane.D
			;
			if (distance < 0)
			{
				result = PlaneIntersectionType.Back;
				return;
			}

			result = PlaneIntersectionType.Intersecting;
		}

		public bool Equals(BoundingBox other) => (Min == other.Min) && (Max == other.Max);

		#endregion

		#region Public Static Methods

		/// <summary>
		/// Create a bounding box from the given list of points.
		/// </summary>
		/// <param name="points">
		/// The list of Vector3 instances defining the point cloud to bound.
		/// </param>
		/// <returns>A bounding box that encapsulates the given point cloud.</returns>
		/// <exception cref="System.ArgumentException">
		/// Thrown if the given list has no points.
		/// </exception>
		public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}

			bool empty = true;
			Vector3 minVec = MaxVector3;
			Vector3 maxVec = MinVector3;
			foreach (Vector3 ptVector in points)
			{
				minVec.X = (minVec.X < ptVector.X) ? minVec.X : ptVector.X;
				minVec.Y = (minVec.Y < ptVector.Y) ? minVec.Y : ptVector.Y;
				minVec.Z = (minVec.Z < ptVector.Z) ? minVec.Z : ptVector.Z;

				maxVec.X = (maxVec.X > ptVector.X) ? maxVec.X : ptVector.X;
				maxVec.Y = (maxVec.Y > ptVector.Y) ? maxVec.Y : ptVector.Y;
				maxVec.Z = (maxVec.Z > ptVector.Z) ? maxVec.Z : ptVector.Z;

				empty = false;
			}
			if (empty)
			{
				throw new ArgumentException("Collection is empty", "points");
			}

			return new BoundingBox(minVec, maxVec);
		}

		public static BoundingBox CreateFromSphere(BoundingSphere sphere)
		{
			CreateFromSphere(ref sphere, out BoundingBox result);
			return result;
		}

		public static void CreateFromSphere(ref BoundingSphere sphere, out BoundingBox result)
		{
			var corner = new Vector3(sphere.Radius);
			result.Min = sphere.Center - corner;
			result.Max = sphere.Center + corner;
		}

		public static BoundingBox CreateMerged(BoundingBox original, BoundingBox additional)
		{
			CreateMerged(ref original, ref additional, out BoundingBox result);
			return result;
		}

		public static void CreateMerged(ref BoundingBox original, ref BoundingBox additional, out BoundingBox result)
		{
			result.Min.X = Math.Min(original.Min.X, additional.Min.X);
			result.Min.Y = Math.Min(original.Min.Y, additional.Min.Y);
			result.Min.Z = Math.Min(original.Min.Z, additional.Min.Z);
			result.Max.X = Math.Max(original.Max.X, additional.Max.X);
			result.Max.Y = Math.Max(original.Max.Y, additional.Max.Y);
			result.Max.Z = Math.Max(original.Max.Z, additional.Max.Z);
		}

		#endregion

		#region Public Static Operators and Override Methods

		public override bool Equals(object obj) => (obj is BoundingBox box) && Equals(box);

		public override int GetHashCode() => Min.GetHashCode() + Max.GetHashCode();

		public static bool operator ==(BoundingBox a, BoundingBox b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(BoundingBox a, BoundingBox b)
		{
			return !a.Equals(b);
		}

		public override string ToString()
		{
			return
				"{{Min:" + Min.ToString() +
				" Max:" + Max.ToString() +
				"}}"
			;
		}

		#endregion
	}
}
