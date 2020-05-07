using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemRandom = System.Random;

namespace EngineeringAcoustics.Curve.Generators
{
	public class NonDeterministicRandom : FunctionGeneratorBase
	{
		private int lastSeed;
		public int Seed
		{
			get
			{
				return lastSeed;
			}
			set
			{
				lastSeed = value;
				random = new SystemRandom(lastSeed);
			}
		}

		public bool SmoothValues { get; set; }

		private SystemRandom random = null;

		public NonDeterministicRandom()
		{
			Seed = 0;
		}

		public void NewSeed()
		{
			random = new SystemRandom();
		}

		public override double Generate(double position)
		{
			double nextNormalized = random.NextDouble();
			double nextScaled = MathExtensions.Lerp(Minimum, Maximum, nextNormalized);
			return nextScaled;
		}
	}
}
