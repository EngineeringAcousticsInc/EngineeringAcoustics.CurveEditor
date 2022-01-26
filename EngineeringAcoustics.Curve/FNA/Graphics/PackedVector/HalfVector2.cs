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
	public struct HalfVector2 : IPackedVector<uint>, IPackedVector, IEquatable<HalfVector2>
	{
		#region Public Properties
		public uint PackedValue { get; set; }

		#endregion

		#region Private Variables


		#endregion

		#region Public Constructors

		public HalfVector2(float x, float y)
		{
			PackedValue = PackHelper(x, y);
		}

		public HalfVector2(Vector2 vector)
		{
			PackedValue = PackHelper(vector.X, vector.Y);
		}

		#endregion

		#region Public Methods

		public Vector2 ToVector2()
		{
			Vector2 vector;
			vector.X = HalfTypeHelper.Convert((ushort)PackedValue);
			vector.Y = HalfTypeHelper.Convert((ushort)(PackedValue >> 0x10));
			return vector;
		}

		#endregion

		#region IPackedVector Methods

		void IPackedVector.PackFromVector4(Vector4 vector) => PackedValue = PackHelper(vector.X, vector.Y);

		Vector4 IPackedVector.ToVector4() => new Vector4(ToVector2(), 0.0f, 1.0f);

		#endregion

		#region Public Static Operators and Override Methods

		public override string ToString() => PackedValue.ToString("X");

		public override int GetHashCode() => PackedValue.GetHashCode();

		public override bool Equals(object obj) => (obj is HalfVector2 vector) && Equals(vector);

		public bool Equals(HalfVector2 other) => PackedValue.Equals(other.PackedValue);

		public static bool operator ==(HalfVector2 a, HalfVector2 b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(HalfVector2 a, HalfVector2 b)
		{
			return !a.Equals(b);
		}

		#endregion

		#region Private Static Pack Method

		private static uint PackHelper(float vectorX, float vectorY) => HalfTypeHelper.Convert(vectorX) |
				((uint)(HalfTypeHelper.Convert(vectorY) << 0x10))
			;

		#endregion
	}
}
