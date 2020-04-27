using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringAcoustics.Curve
{
	public interface IComplexCurveMutator
	{
		bool IsConstant { get; }
		string Name { get; }
		double Mutate(double position, double input);
	}
}
