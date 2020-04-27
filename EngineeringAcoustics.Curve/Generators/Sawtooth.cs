using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringAcoustics.Curve.Generators
{
	public class Sawtooth : WaveGenerator
	{
		protected override double GenerateNormalizedWave(double position)
		{
			return Math.Abs(position % WavePeriod);
		}
	}
}
