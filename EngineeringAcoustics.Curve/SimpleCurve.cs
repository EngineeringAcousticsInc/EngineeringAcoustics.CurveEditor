using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XnaCurve = Microsoft.Xna.Framework.Curve;

namespace EngineeringAcoustics.Curve
{
	public class SimpleCurve : XnaCurve, ICurve
	{
		public SimpleCurve() { }

		public ICurve BaseCurve => null;

		ICurve /*ICurve. #clone */Clone()
		{
			var clone = new SimpleCurve();

			foreach (var key in Keys)
			{
				clone.Keys.Add(key);
			}

			clone.PreLoop = PreLoop;
			clone.PostLoop = PostLoop;

			return clone;
		}
	}
}
