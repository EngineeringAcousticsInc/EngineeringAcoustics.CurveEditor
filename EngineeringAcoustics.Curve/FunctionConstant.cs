using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringAcoustics.Curve
{
	public class FunctionConstant : IFunctionGenerator
	{
		public virtual bool IsConstant => true;
		public virtual string Name => "Constant";
		public virtual float Value { get; set; } = 1.0f;
		public virtual float Generate(float position) => Value;
	}
}
