
using SystemRandom = System.Random;

namespace EngineeringAcoustics.Curve.Generators
{
	public class NonDeterministicRandom : FunctionGeneratorBase
	{
		private int _lastSeed;
		public int Seed
		{
			get => _lastSeed;
			set
			{
				_lastSeed = value;
				_random = new SystemRandom(_lastSeed);
			}
		}

		public bool SmoothValues { get; set; }

		private SystemRandom _random = null;

		public NonDeterministicRandom()
		{
			Seed = 0;
		}

		public void NewSeed() => _random = new SystemRandom();

		public override double Generate(double position)
		{
			double nextNormalized = _random.NextDouble();
			double nextScaled = MathExtensions.Lerp(Minimum, Maximum, nextNormalized);
			return nextScaled;
		}
	}
}
