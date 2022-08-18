using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod.Interpolation
{
	public class Vector3Interpolator<T> where T : Interpolator, new()
	{
		public T XInterpolator;
		public T YInterpolator;
		public T ZInterpolator;


		public Vector3Interpolator()
		{
			XInterpolator = new T();
			YInterpolator = new T();
			ZInterpolator = new T();
		}

		public void Teleport(Vector3 value)
		{
			XInterpolator.Teleport(value.x);
			YInterpolator.Teleport(value.y);
			ZInterpolator.Teleport(value.z);
		}

		public Vector3 GetCurrentValue(float deltaTime)
		{
			return new Vector3(
				XInterpolator.GetCurrentValue(deltaTime),
				YInterpolator.GetCurrentValue(deltaTime),
				ZInterpolator.GetCurrentValue(deltaTime));
		}

		public void SetTarget(Vector3 value)
		{
			XInterpolator.SetTarget(value.x);
			YInterpolator.SetTarget(value.y);
			ZInterpolator.SetTarget(value.z);
		}
	}
}
