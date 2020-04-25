using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringAcoustics.Curve
{
	public interface IFunctionGenerator
	{
		bool IsConstant { get; }
		string Name { get; }
		float Generate(float position);
	}
}
