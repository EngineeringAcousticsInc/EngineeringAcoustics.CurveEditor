using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringAcoustics.Curve.Generators
{
	public class Constant : IFunctionGenerator
	{
		public virtual bool IsConstant => true;
		public virtual string Name => "Constant";
		public virtual double Value { get; set; } = 1.0;
		public virtual double Generate(double position) => Value;
	}
}
