namespace EngineeringAcoustics.Curve
{
	public class ComplexCurvePassthruMutator : IComplexCurveMutator
	{
		public virtual bool IsConstant => true;
		public virtual string Name => "None";
		public virtual double Mutate(double position, double input) => input;
		public override string ToString() => Name;
	}
}
