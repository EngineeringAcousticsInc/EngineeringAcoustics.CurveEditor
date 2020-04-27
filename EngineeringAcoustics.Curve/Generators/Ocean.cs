using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringAcoustics.Curve.Generators
{
	public class Ocean : WaveGenerator
	{
		public override double WavePeriod => Math.PI * 2.0;

		protected override double GenerateNormalizedWave(double position)
		{
			return (float)Math.Sin(position * WavePeriod);
		}
	}
}
