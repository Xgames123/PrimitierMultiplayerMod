using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared
{
	public static class MathF
	{
		public static float Floor(float x)
		{
#if UNITY_MATH
			return UnityEngine.Mathf.Floor(x);
#elif NET_MATH
			return System.MathF.Floor(x);
#endif

		}

	}
}
