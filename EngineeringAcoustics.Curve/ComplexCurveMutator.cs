using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringAcoustics.Curve
{
	public class ComplexCurveMutator : IComplexCurveMutator
	{
		public virtual bool IsConstant => Function?.IsConstant ?? true;

		public virtual ComplexCurveMutatorOperation Operation { get; set; } = ComplexCurveMutatorOperation.Multiply;

		public virtual string Name => Operation.ToString();

		public virtual IFunctionGenerator Function { get; set; } = new FunctionConstant();

		public virtual float Mutate(float position, float input)
		{
			if (Function == null)
			{
				return input;
			}

			float functionValue = Function.Generate(position);

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
