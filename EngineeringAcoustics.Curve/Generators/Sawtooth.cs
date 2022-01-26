using System;

namespace EngineeringAcoustics.Curve.Generators
{
	public class Sawtooth : WaveGenerator
	{
		protected override double GenerateNormalizedWave(double position) => Math.Abs(position % WavePeriod);
	}
}
