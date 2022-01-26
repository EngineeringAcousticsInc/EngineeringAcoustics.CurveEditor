namespace EngineeringAcoustics.Curve.Generators
{
	public abstract class WaveGenerator : FunctionGeneratorBase
	{
		public virtual double WavePeriod => 1.0;
		public virtual double WaveAmplitude => 1.0;
		protected abstract double GenerateNormalizedWave(double position);

		public double Assimetry { get; set; } = 0f;
		public double Offset { get; set; } = 0f;

		public override bool IsConstant => base.IsConstant || Assimetry >= 1f;

		public override double Generate(double position)
		{
			double offsetPosition = position + Offset;
			double normalizedValue = GenerateNormalizedWave(offsetPosition);
			double scaledValue = normalizedValue * WaveAmplitude; // #generate
			return scaledValue;
		}
	}
}
