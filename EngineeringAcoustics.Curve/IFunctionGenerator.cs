namespace EngineeringAcoustics.Curve
{
	public interface IFunctionGenerator
	{
		bool IsConstant { get; }
		string Name { get; }
		double Generate(double position);
	}
}
