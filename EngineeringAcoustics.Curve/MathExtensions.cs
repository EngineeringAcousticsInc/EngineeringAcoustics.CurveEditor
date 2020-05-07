using static System.Math;

namespace EngineeringAcoustics.Curve
{
	internal static class MathExtensions
	{
		public static double Lerp(this double value1, double value2, double amount)
		{
			return value1 + (value2 - value1) * amount;
		}

		public static float Lerp(this float value1, float value2, float amount)
		{
			return value1 + (value2 - value1) * amount;
		}

		public static double InverseLerp(this double min, double max, double value)
		{
			if (Abs(max - min) < double.Epsilon)
			{
				return min;
			}

			return (value - min) / (max - min);
		}
	}
}
