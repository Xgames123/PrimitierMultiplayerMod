using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimitierMultiplayerMod.Interpolation
{
	public class SmoothInterpolator : Interpolator
	{
		public Queue<float> Targets = new Queue<float>(5);
		public float Value = 0;

		private float _velosity = 0;
		private float _currentTarget = 0;

		public override float GetCurrentValue(float deltaTime)
		{

			var smoothTime = 0.5f - (0.2f * Targets.Count);
			if(smoothTime <= 0)
			{
				smoothTime = 0.05f;
			}
			Value = Mathf.SmoothDamp(Value, _currentTarget, ref _velosity, smoothTime, 100f, deltaTime);


			if(Value > _currentTarget-0.2f)
			{
				if (Targets.Count == 0)
					return Value;

				_currentTarget = Targets.Dequeue();
			}



			return Value;
		}

		public override void SetTarget(float value)
		{
			Targets.Enqueue(value);
		}
	}
}
