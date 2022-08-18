using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod.Interpolation
{
	public class SmoothInterpolator : Interpolator
	{
		public float Velosity;
		public List<float> Targets = new List<float>(5);
		public float LastTarget = 0;
		public float Value;

		public override float GetCurrentValue(float deltaTime)
		{
			Value += Velosity;

			var error = Targets[0] - Value;

			var length = (Targets[0] - LastTarget);

			Value += error / 2;

			var progress = (length - error)/length;

			return Value;
		}

		public override void SetTarget(float value)
		{
			Targets.Add(value);
		}
	}
}
