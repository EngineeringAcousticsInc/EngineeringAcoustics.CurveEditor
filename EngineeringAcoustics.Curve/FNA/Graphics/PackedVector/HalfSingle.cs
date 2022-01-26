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
		public ushort PackedValue { get; set; }

		#endregion

		#region Private Variables


		#endregion

		#region Public Constructors

		public HalfSingle(float single)
		{
			PackedValue = HalfTypeHelper.Convert(single);
		}

		#endregion

		#region Public Methods

		public float ToSingle() => HalfTypeHelper.Convert(PackedValue);

		#endregion

		#region IPackedVector Methods

		void IPackedVector.PackFromVector4(Vector4 vector) => PackedValue = HalfTypeHelper.Convert(vector.X);

		Vector4 IPackedVector.ToVector4() => new Vector4(ToSingle(), 0.0f, 0.0f, 1.0f);

		#endregion

		#region Public Static Operators and Override Methods

		public override bool Equals(object obj) => (obj is HalfSingle single) && Equals(single);

		public bool Equals(HalfSingle other) => PackedValue == other.PackedValue;

		public override string ToString() => PackedValue.ToString("X");

		public override int GetHashCode() => PackedValue.GetHashCode();

		public static bool operator ==(HalfSingle lhs, HalfSingle rhs)
		{
			return lhs.PackedValue == rhs.PackedValue;
		}

		public static bool operator !=(HalfSingle lhs, HalfSingle rhs)
		{
			return lhs.PackedValue != rhs.PackedValue;
		}

		#endregion
	}
}
