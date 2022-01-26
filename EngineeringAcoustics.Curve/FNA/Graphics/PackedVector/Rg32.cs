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
	public struct Rg32 : IPackedVector<uint>, IEquatable<Rg32>, IPackedVector
	{
		#region Public Properties

		/// <summary>
		/// Gets and sets the packed value.
		/// </summary>
		public uint PackedValue { get; set; }

		#endregion

		#region Private Variables


		#endregion

		#region Public Constructors

		/// <summary>
		/// Creates a new instance of Rg32.
		/// </summary>
		/// <param name="x">The x component</param>
		/// <param name="y">The y component</param>
		public Rg32(float x, float y)
		{
			PackedValue = Pack(x, y);
		}

		/// <summary>
		/// Creates a new instance of Rg32.
		/// </summary>
		/// <param name="vector">
		/// Vector containing the components for the packed vector.
		/// </param>
		public Rg32(Vector2 vector)
		{
			PackedValue = Pack(vector.X, vector.Y);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets the packed vector in Vector2 format.
		/// </summary>
		/// <returns>The packed vector in Vector2 format</returns>
		public Vector2 ToVector2() => new Vector2(
				(PackedValue & 0xFFFF) / 65535.0f,
				(PackedValue >> 16) / 65535.0f
			);

		#endregion

		#region IPackedVector Methods

		/// <summary>
		/// Sets the packed vector from a Vector4.
		/// </summary>
		/// <param name="vector">Vector containing the components.</param>
		void IPackedVector.PackFromVector4(Vector4 vector) => PackedValue = Pack(vector.X, vector.Y);

		/// <summary>
		/// Gets the packed vector in Vector4 format.
		/// </summary>
		/// <returns>The packed vector in Vector4 format</returns>
		Vector4 IPackedVector.ToVector4() => new Vector4(ToVector2(), 0.0f, 1.0f);

		#endregion

		#region Public Static Operators and Override Methods

		/// <summary>
		/// Compares an object with the packed vector.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns>True if the object is equal to the packed vector.</returns>
		public override bool Equals(object obj) => (obj is Rg32 rg) && Equals(rg);

		/// <summary>
		/// Compares another Rg32 packed vector with the packed vector.
		/// </summary>
		/// <param name="other">The Rg32 packed vector to compare.</param>
		/// <returns>True if the packed vectors are equal.</returns>
		public bool Equals(Rg32 other) => PackedValue == other.PackedValue;

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

		public static bool operator ==(Rg32 lhs, Rg32 rhs)
		{
			return lhs.PackedValue == rhs.PackedValue;
		}

		public static bool operator !=(Rg32 lhs, Rg32 rhs)
		{
			return lhs.PackedValue != rhs.PackedValue;
		}

		#endregion

		#region Private Static Pack Method

		private static uint Pack(float x, float y) => ((uint)Math.Round(MathHelper.Clamp(x, 0, 1) * 65535.0f)) |
				(((uint)Math.Round(MathHelper.Clamp(y, 0, 1) * 65535.0f)) << 16)
			;

		#endregion
	}
}
