﻿using System;

using EngineeringAcoustics.Curve.Generators;

namespace EngineeringAcoustics.Curve
{
	public class ComplexCurveMutator : IComplexCurveMutator
	{
		public virtual bool IsConstant => Function?.IsConstant ?? true;

		public virtual ComplexCurveMutatorOperation Operation { get; set; } = ComplexCurveMutatorOperation.Multiply;

		public virtual string Name => Operation.ToString();

		public virtual IFunctionGenerator Function { get; set; } = new Constant();

		public virtual double Mutate(double position, double input)
		{
			if (Function == null)
			{
				return input;
			}

			double functionValue = Function.Generate(position);

			switch (Operation)
			{
				case ComplexCurveMutatorOperation.Multiply:
					return input * functionValue;
				case ComplexCurveMutatorOperation.Add:
					return input + functionValue;
				case ComplexCurveMutatorOperation.Subtract:
					return input - functionValue;
				default:
					throw new NotImplementedException($"Operation {Operation} has no case in {nameof(Mutate)}");
			}
		}

		public override string ToString() => Name;
	}
}
