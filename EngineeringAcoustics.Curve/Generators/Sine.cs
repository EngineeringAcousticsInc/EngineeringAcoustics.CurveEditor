using System;

namespace EngineeringAcoustics.Curve.Generators
{
	public class Sine : WaveGenerator
	{
		public override double WavePeriod => Math.PI * 2.0;

		protected override double GenerateNormalizedWave(double position) => (float)Math.Sin(position * WavePeriod);
	}
}
