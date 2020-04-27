using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringAcoustics.Curve.Generators
{
	public abstract class FunctionGeneratorBase : IFunctionGenerator
	{
		public virtual bool IsConstant => Minimum != Maximum || Frequency.Equals(0.0);
		public virtual string Name => GetType().Name;
		public virtual double Minimum { get; set; } = 0.0;
		public virtual double Maximum { get; set; } = 1.0;
		public virtual double Frequency { get; set; } = 1.0;
		public override string ToString() => Name;

		public abstract double Generate(double position);
	}
}
