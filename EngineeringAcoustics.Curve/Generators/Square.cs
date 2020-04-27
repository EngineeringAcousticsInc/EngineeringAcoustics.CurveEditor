using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringAcoustics.Curve.Generators
{
	public class Square : WaveGenerator
	{
		protected override double GenerateNormalizedWave(double position)
		{
			return (position % WavePeriod) <= (WavePeriod / 2.0) ? 1.0 : -1.0;
		}
	}
}
