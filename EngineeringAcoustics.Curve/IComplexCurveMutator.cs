namespace EngineeringAcoustics.Curve
{
	public interface IComplexCurveMutator
	{
		bool IsConstant { get; }
		string Name { get; }
		double Mutate(double position, double input);
	}
}
