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
	public struct Short2 : IPackedVector<uint>, IEquatable<Short2>
	{
		#region Public Properties
		public uint PackedValue { get; set; }

		#endregion

		#region Private Variables


		#endregion

		#region Public Constructors

		public Short2(Vector2 vector)
		{
			PackedValue = Pack(vector.X, vector.Y);
		}

		public Short2(float x, float y)
		{
			PackedValue = Pack(x, y);
		}

		#endregion

		#region Public Methods

		public Vector2 ToVector2() => new Vector2(
				(short)(PackedValue & 0xFFFF),
				(short)(PackedValue >> 16)
			);

		#endregion

		#region IPackedVector Methods

		void IPackedVector.PackFromVector4(Vector4 vector) => PackedValue = Pack(vector.X, vector.Y);

		Vector4 IPackedVector.ToVector4() => new Vector4(ToVector2(), 0.0f, 1.0f);

		#endregion

		#region Public Static Operators and Override Methods

		public static bool operator !=(Short2 a, Short2 b)
		{
			return a.PackedValue != b.PackedValue;
		}

		public static bool operator ==(Short2 a, Short2 b)
		{
			return a.PackedValue == b.PackedValue;
		}

		public override bool Equals(object obj) => (obj is Short2 short2) && Equals(short2);

		public bool Equals(Short2 other) => this == other;

		public override int GetHashCode() => PackedValue.GetHashCode();

		public override string ToString() => PackedValue.ToString("X");

		#endregion

		#region Private Static Pack Method

		private static uint Pack(float x, float y) => (uint)(
				((int)Math.Round(MathHelper.Clamp(x, -32768, 32767)) & 0x0000FFFF) |
				(((int)Math.Round(MathHelper.Clamp(y, -32768, 32767))) << 16)
			);

		#endregion
	}
}
