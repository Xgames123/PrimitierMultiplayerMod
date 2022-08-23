using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Mod.Interpolation
{
	public abstract class Interpolator
	{

		public abstract void Teleport(float value);

		public abstract void SetTarget(float value);


		public abstract float GetCurrentValue(float deltaTime);

	}
}
