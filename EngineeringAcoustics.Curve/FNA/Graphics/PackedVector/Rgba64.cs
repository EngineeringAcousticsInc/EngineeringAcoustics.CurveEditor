#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2020 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
#endregion

namespace Microsoft.Xna.Framework.Graphics.PackedVector
{
	/// <summary>
	/// Packed vector type containing unsigned normalized values ranging from 0 to 1. 
	/// The x and z components use 5 bits, and the y component uses 6 bits.
	/// </summary>
	public struct Rgba64 : IPackedVector<ulong>, IEquatable<Rgba64>, IPackedVector
	{
		#region Public Properties

		/// <summary>
		/// Gets and sets the packed value.
		/// </summary>
		public ulong PackedValue { get; set; }

		#endregion

		#region Private Variables


		#endregion

		#region Public Constructors

		/// <summary>
		/// Creates a new instance of Rgba64.
		/// </summary>
		/// <param name="x">The x component</param>
		/// <param name="y">The y component</param>
		/// <param name="z">The z component</param>
		/// <param name="w">The w component</param>
		public Rgba64(float x, float y, float z, float w)
		{
			PackedValue = Pack(x, y, z, w);
		}

		/// <summary>
		/// Creates a new instance of Rgba64.
		/// </summary>
		/// <param name="vector">
		/// Vector containing the components for the packed vector.
		/// </param>
		public Rgba64(Vector4 vector)
		{
			PackedValue = Pack(vector.X, vector.Y, vector.Z, vector.W);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets the packed vector in Vector4 format.
		/// </summary>
		/// <returns>The packed vector in Vector4 format</returns>
		public Vector4 ToVector4() => new Vector4(
				(PackedValue & 0xFFFF) / 65535.0f,
				((PackedValue >> 16) & 0xFFFF) / 65535.0f,
				((PackedValue >> 32) & 0xFFFF) / 65535.0f,
				(PackedValue >> 48) / 65535.0f
			);

		#endregion

		#region IPackedVector Methods

		/// <summary>
		/// Sets the packed vector from a Vector4.
		/// </summary>
		/// <param name="vector">Vector containing the components.</param>
		void IPackedVector.PackFromVector4(Vector4 vector) => PackedValue = Pack(vector.X, vector.Y, vector.Z, vector.W);

		#endregion

		#region Public Static Operators and Override Methods

		/// <summary>
		/// Compares an object with the packed vector.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns>True if the object is equal to the packed vector.</returns>
		public override bool Equals(object obj) => (obj is Rgba64 rgba) && Equals(rgba);

		/// <summary>
		/// Compares another Rgba64 packed vector with the packed vector.
		/// </summary>
		/// <param name="other">The Rgba64 packed vector to compare.</param>
		/// <returns>True if the packed vectors are equal.</returns>
		public bool Equals(Rgba64 other) => PackedValue == other.PackedValue;

		/// <summary>
		/// Gets a string representation of the packed vector.
		/// </summary>
		/// <returns>A string representation of the packed vector.</returns>
		public override string ToString() => PackedValue.ToString("X");

		/// <summary>
		/// Gets a hash code of the packed vector.
		/// </summary>
		/// <returns>The hash code for the packed vector.</returns>
		public override int GetHashCode() => PackedValue.GetHashCode();

		public static bool operator ==(Rgba64 lhs, Rgba64 rhs)
		{
			return lhs.PackedValue == rhs.PackedValue;
		}

		public static bool operator !=(Rgba64 lhs, Rgba64 rhs)
		{
			return lhs.PackedValue != rhs.PackedValue;
		}

		#endregion

		#region Private Static Pack Method

		private static ulong Pack(float x, float y, float z, float w) => ((ulong)Math.Round(MathHelper.Clamp(x, 0, 1) * 65535.0f)) |
				(((ulong)Math.Round(MathHelper.Clamp(y, 0, 1) * 65535.0f)) << 16) |
				(((ulong)Math.Round(MathHelper.Clamp(z, 0, 1) * 65535.0f)) << 32) |
				(((ulong)Math.Round(MathHelper.Clamp(w, 0, 1) * 65535.0f)) << 48)
			;

		#endregion
	}
}
