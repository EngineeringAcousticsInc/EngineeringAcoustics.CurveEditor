namespace EngineeringAcoustics.Curve.Generators
{
	public class Square : WaveGenerator
	{
		protected override double GenerateNormalizedWave(double position) => (position % WavePeriod) <= (WavePeriod / 2.0) ? 1.0 : -1.0;
	}
}
