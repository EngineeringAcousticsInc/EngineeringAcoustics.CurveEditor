using System;

using Microsoft.Xna.Framework;

namespace EngineeringAcoustics.Curve
{
	public sealed class ComplexCurve : ICurve
	{
		public bool IsConstant => BaseCurve.IsConstant && Mutator == null;

		public ICurve BaseCurve { get; private set; } = new SimpleCurve();
		public CurveKeyCollection Keys => BaseCurve.Keys;
		public CurveLoopType PreLoop { get => BaseCurve.PreLoop; set => BaseCurve.PreLoop = value; }
		public CurveLoopType PostLoop { get => BaseCurve.PostLoop; set => BaseCurve.PostLoop = value; }

		public float MinimumPosition { get; set; } = 0.0f;
		public float MaximumPosition { get; set; } = 1.0f;
		public float MinimumValue { get; set; } = 0.0f;
		public float MaximumValue { get; set; } = 1.0f;

		public IComplexCurveMutator Mutator { get; set; } = new ComplexCurvePassthruMutator();

		public ComplexCurve()
		{

		}

		public /*ICurve. #clone*/ComplexCurve Clone() =>
			// #clone
			throw new NotImplementedException("NYI #clone");

		public float Evaluate(float position)
		{
			float curveValue = BaseCurve.Evaluate(position);
			float mutatedValue = (float)Mutator.Mutate(position, curveValue);

			// todo, handle position loop for mutator?

			return mutatedValue;
		}

		public void ComputeTangents(CurveTangent tangentType)
			=> ComputeTangents(tangentType, tangentType);
		public void ComputeTangents(CurveTangent tangentInType, CurveTangent tangentOutType)
			=> BaseCurve.ComputeTangents(tangentInType, tangentOutType);
		public void ComputeTangent(int keyIndex, CurveTangent tangentType)
			=> ComputeTangent(keyIndex, tangentType, tangentType);
		public void ComputeTangent(int keyIndex, CurveTangent tangentInType, CurveTangent tangentOutType)
			=> BaseCurve.ComputeTangent(keyIndex, tangentInType, tangentOutType);
	}
}
