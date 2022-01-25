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
	public struct HalfSingle : IPackedVector<ushort>, IEquatable<HalfSingle>, IPackedVector
	{
		#region Public Properties
		public ushort PackedValue
		{
			get => packedValue;
			set => packedValue = value;
		}

		#endregion

		#region Private Variables

		private ushort packedValue;

		#endregion

		#region Public Constructors

		public HalfSingle(float single)
		{
			packedValue = HalfTypeHelper.Convert(single);
		}

		#endregion

		#region Public Methods

		public float ToSingle() => HalfTypeHelper.Convert(packedValue);

		#endregion

		#region IPackedVector Methods

		void IPackedVector.PackFromVector4(Vector4 vector) => packedValue = HalfTypeHelper.Convert(vector.X);

		Vector4 IPackedVector.ToVector4() => new Vector4(ToSingle(), 0.0f, 0.0f, 1.0f);

		#endregion

		#region Public Static Operators and Override Methods

		public override bool Equals(object obj) => (obj is HalfSingle) && Equals((HalfSingle)obj);

		public bool Equals(HalfSingle other) => packedValue == other.packedValue;

		public override string ToString() => packedValue.ToString("X");

		public override int GetHashCode() => packedValue.GetHashCode();

		public static bool operator ==(HalfSingle lhs, HalfSingle rhs)
		{
			return lhs.packedValue == rhs.packedValue;
		}

		public static bool operator !=(HalfSingle lhs, HalfSingle rhs)
		{
			return lhs.packedValue != rhs.packedValue;
		}

		#endregion
	}
}
