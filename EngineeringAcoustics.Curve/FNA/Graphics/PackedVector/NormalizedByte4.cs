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
	public struct NormalizedByte4 : IPackedVector<uint>, IEquatable<NormalizedByte4>
	{
		#region Public Properties
		public uint PackedValue { get; set; }

		#endregion

		#region Private Variables


		#endregion

		#region Public Constructors

		public NormalizedByte4(Vector4 vector)
		{
			PackedValue = Pack(vector.X, vector.Y, vector.Z, vector.W);
		}

		public NormalizedByte4(float x, float y, float z, float w)
		{
			PackedValue = Pack(x, y, z, w);
		}

		#endregion

		#region Public Methods

		public Vector4 ToVector4() => new Vector4(
				((sbyte)(PackedValue & 0xFF)) / 127.0f,
				((sbyte)((PackedValue >> 8) & 0xFF)) / 127.0f,
				((sbyte)((PackedValue >> 16) & 0xFF)) / 127.0f,
				((sbyte)((PackedValue >> 24) & 0xFF)) / 127.0f
			);

		#endregion

		#region IPackedVector Methods

		void IPackedVector.PackFromVector4(Vector4 vector) => PackedValue = Pack(vector.X, vector.Y, vector.Z, vector.W);

		#endregion

		#region Public Static Operators and Override Methods

		public static bool operator !=(NormalizedByte4 a, NormalizedByte4 b)
		{
			return a.PackedValue != b.PackedValue;
		}

		public static bool operator ==(NormalizedByte4 a, NormalizedByte4 b)
		{
			return a.PackedValue == b.PackedValue;
		}

		public override bool Equals(object obj) => (obj is NormalizedByte4 byte4) && Equals(byte4);

		public bool Equals(NormalizedByte4 other) => PackedValue == other.PackedValue;

		public override int GetHashCode() => PackedValue.GetHashCode();

		public override string ToString() => PackedValue.ToString("X");

		#endregion

		#region Private Static Pack Method

		private static uint Pack(float x, float y, float z, float w)
		{
			uint byte4 = (
				(uint)Math.Round(MathHelper.Clamp(x, -1.0f, 1.0f) * 127.0f)
			) & 0x000000FF;
			uint byte3 = (
				(
					(uint)Math.Round(MathHelper.Clamp(y, -1.0f, 1.0f) * 127.0f)
				) << 8
			) & 0x0000FF00;
			uint byte2 = (
				(
					(uint)Math.Round(MathHelper.Clamp(z, -1.0f, 1.0f) * 127.0f)
				) << 16
			) & 0x00FF0000;
			uint byte1 = (
				(
					(uint)Math.Round(MathHelper.Clamp(w, -1.0f, 1.0f) * 127.0f)
				) << 24
			) & 0xFF000000;

			return byte4 | byte3 | byte2 | byte1;
		}

		#endregion
	}
}
