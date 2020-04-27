using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringAcoustics.Curve.Generators
{
	public class Triangle : WaveGenerator
	{
		public override double WavePeriod => Math.PI * 2.0;
		public override double WaveAmplitude => Math.PI / 2.0;

		protected override double GenerateNormalizedWave(double position)
		{
			return Math.Asin(Math.Sin(position * WavePeriod)) / (WaveAmplitude);
		}
	}
}
