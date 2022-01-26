using System;

namespace EngineeringAcoustics.Curve.Generators
{
	public class Triangle : WaveGenerator
	{
		public override double WavePeriod => Math.PI * 2.0;
		public override double WaveAmplitude => Math.PI / 2.0;

		protected override double GenerateNormalizedWave(double position) => Math.Asin(Math.Sin(position * WavePeriod)) / WaveAmplitude;
	}
}
