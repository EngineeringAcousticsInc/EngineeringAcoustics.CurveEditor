
using XnaCurve = Microsoft.Xna.Framework.Curve;

namespace EngineeringAcoustics.Curve
{
	public class SimpleCurve : XnaCurve, ICurve
	{
		public SimpleCurve() { }

		public ICurve BaseCurve => null;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unused as of yet.")]
		private ICurve /*ICurve. #clone */CloneNyi()
		{
			var clone = new SimpleCurve();

			foreach (Microsoft.Xna.Framework.CurveKey key in Keys)
			{
				clone.Keys.Add(key);
			}

			clone.PreLoop = PreLoop;
			clone.PostLoop = PostLoop;

			return clone;
		}
	}
}
