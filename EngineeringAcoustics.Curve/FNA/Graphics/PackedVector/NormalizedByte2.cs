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
	public struct NormalizedByte2 : IPackedVector<ushort>, IEquatable<NormalizedByte2>
	{
		#region Public Properties
		public ushort PackedValue { get; set; }

		#endregion

		#region Private Variables


		#endregion

		#region Public Constructors

		public NormalizedByte2(Vector2 vector)
		{
			PackedValue = Pack(vector.X, vector.Y);
		}

		public NormalizedByte2(float x, float y)
		{
			PackedValue = Pack(x, y);
		}

		#endregion

		#region Public Methods

		public Vector2 ToVector2() => new Vector2(
				((sbyte)(PackedValue & 0xFF)) / 127.0f,
				((sbyte)((PackedValue >> 8) & 0xFF)) / 127.0f
			);

		#endregion

		#region IPackedVector Methods

		void IPackedVector.PackFromVector4(Vector4 vector) => PackedValue = Pack(vector.X, vector.Y);

		Vector4 IPackedVector.ToVector4() => new Vector4(ToVector2(), 0.0f, 1.0f);

		#endregion

		#region Public Static Operators and Override Methods

		public static bool operator !=(NormalizedByte2 a, NormalizedByte2 b)
		{
			return a.PackedValue != b.PackedValue;
		}

		public static bool operator ==(NormalizedByte2 a, NormalizedByte2 b)
		{
			return a.PackedValue == b.PackedValue;
		}

		public override bool Equals(object obj) => (obj is NormalizedByte2 byte2) && Equals(byte2);

		public bool Equals(NormalizedByte2 other) => PackedValue == other.PackedValue;

		public override int GetHashCode() => PackedValue.GetHashCode();

		public override string ToString() => PackedValue.ToString("X");

		#endregion

		#region Private Static Pack Method

		private static ushort Pack(float x, float y)
		{
			int byte2 = (
				(ushort)
					Math.Round(MathHelper.Clamp(x, -1.0f, 1.0f) * 127.0f)

			) & 0x00FF;
			int byte1 = (
				((ushort)
					Math.Round(MathHelper.Clamp(y, -1.0f, 1.0f) * 127.0f)
				) << 8
			) & 0xFF00;

			return (ushort)(byte2 | byte1);
		}

		#endregion
	}
}
