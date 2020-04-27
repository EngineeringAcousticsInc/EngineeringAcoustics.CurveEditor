using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringAcoustics.Curve.Generators
{
	// #fft needs input
	class Fft : IFunctionGenerator
	{
		public bool IsConstant => Frequency.Equals(0f) || Scale.Equals(0f);
		public string Name => "FFT";

		public float Frequency { get; set; } = 1f;
		public float Scale { get; set; } = 1f;
		public float Bias { get; set; } = 0f;
		public float SmoothSteps { get; set; } = 1f;

		public double Generate(double position)
		{
			// #fft implement algo
			return 0f;
		}
	}
}
