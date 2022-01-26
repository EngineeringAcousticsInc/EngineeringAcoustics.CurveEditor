using Microsoft.Xna.Framework;

namespace EngineeringAcoustics.Curve
{
	public interface ICurve
	{
		/// <summary>
		/// Returns <c>true</c> if this curve is constant (has zero or one points); <c>false</c> otherwise.
		/// </summary>
		bool IsConstant { get; }

		/// <summary>
		/// The curve's base curve, if it has one. Null otherwise.
		/// </summary>
		ICurve BaseCurve { get; }

		/// <summary>
		/// The collection of curve keys.
		/// </summary>
		CurveKeyCollection Keys { get; }

		/// <summary>
		/// Defines how to handle weighting values that are greater than the last control point in the curve.
		/// </summary>
		CurveLoopType PostLoop { get; set; }

		/// <summary>
		/// Defines how to handle weighting values that are less than the first control point in the curve.
		/// </summary>
		CurveLoopType PreLoop { get; set; }

		/// <summary>
		/// Creates a copy of this curve.
		/// </summary>
		/// <returns>A copy of this curve.</returns>
		//ICurve Clone(); #clone

		/// <summary>
		/// Evaluate the value at a position of this <see cref="Curve"/>.
		/// </summary>
		/// <param name="position">The position on this <see cref="Curve"/>.</param>
		/// <returns>Value at the position on this <see cref="Curve"/>.</returns>
		float Evaluate(float position);

		/// <summary>
		/// Computes tangents for all keys in the collection.
		/// </summary>
		/// <param name="tangentType">The tangent type for both in and out.</param>
		void ComputeTangents(CurveTangent tangentType);

		/// <summary>
		/// Computes tangents for all keys in the collection.
		/// </summary>
		/// <param name="tangentInType">The tangent in-type. <see cref="CurveKey.TangentIn"/> for more details.</param>
		/// <param name="tangentOutType">The tangent out-type. <see cref="CurveKey.TangentOut"/> for more details.</param>
		void ComputeTangents(CurveTangent tangentInType, CurveTangent tangentOutType);

		/// <summary>
		/// Computes tangent for the specific key in the collection.
		/// </summary>
		/// <param name="keyIndex">The index of a key in the collection.</param>
		/// <param name="tangentType">The tangent type for both in and out.</param>
		void ComputeTangent(int keyIndex, CurveTangent tangentType);

		/// <summary>
		/// Computes tangent for the specific key in the collection.
		/// </summary>
		/// <param name="keyIndex">The index of key in the collection.</param>
		/// <param name="tangentInType">The tangent in-type. <see cref="CurveKey.TangentIn"/> for more details.</param>
		/// <param name="tangentOutType">The tangent out-type. <see cref="CurveKey.TangentOut"/> for more details.</param>
		void ComputeTangent(int keyIndex, CurveTangent tangentInType, CurveTangent tangentOutType);
	}
}
