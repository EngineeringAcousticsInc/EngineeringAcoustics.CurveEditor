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
	public struct NormalizedShort2 : IPackedVector<uint>, IEquatable<NormalizedShort2>
	{
		#region Public Properties
		public uint PackedValue { get; set; }

		#endregion

		#region Private Variables


		#endregion

		#region Public Constructors

		public NormalizedShort2(Vector2 vector)
		{
			PackedValue = Pack(vector.X, vector.Y);
		}

		public NormalizedShort2(float x, float y)
		{
			PackedValue = Pack(x, y);
		}

		#endregion

		#region Public Methods

		public Vector2 ToVector2()
		{
			const float maxVal = 0x7FFF;

			return new Vector2(
				(short)(PackedValue & 0xFFFF) / maxVal,
				(short)(PackedValue >> 0x10) / maxVal
			);
		}

		#endregion

		#region IPackedVector Methods

		void IPackedVector.PackFromVector4(Vector4 vector) => PackedValue = Pack(vector.X, vector.Y);

		Vector4 IPackedVector.ToVector4() => new Vector4(ToVector2(), 0.0f, 1.0f);

		#endregion

		#region Public Static Operators and Override Methods

		public static bool operator !=(NormalizedShort2 a, NormalizedShort2 b)
		{
			return !a.Equals(b);
		}

		public static bool operator ==(NormalizedShort2 a, NormalizedShort2 b)
		{
			return a.Equals(b);
		}

		public override bool Equals(object obj) => (obj is NormalizedShort2 short2) && Equals(short2);

		public bool Equals(NormalizedShort2 other) => PackedValue.Equals(other.PackedValue);

		public override int GetHashCode() => PackedValue.GetHashCode();

		public override string ToString() => PackedValue.ToString("X");

		#endregion

		#region Private Static Pack Method

		private static uint Pack(float x, float y)
		{
			const float max = 0x7FFF;
			const float min = -max;

			uint word2 = (uint)(
				(int)MathHelper.Clamp(
					(float)Math.Round(x * max),
					min,
					max
				) & 0xFFFF
			);
			uint word1 = (uint)((
				(int)MathHelper.Clamp(
					(float)Math.Round(y * max),
					min,
					max
				) & 0xFFFF
			) << 0x10);

			return word2 | word1;
		}

		#endregion
	}
}
