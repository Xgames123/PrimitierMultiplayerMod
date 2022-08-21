using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimitierMultiplayer.Mod.Interpolation
{
	public class SmoothInterpolator : Interpolator
	{
		public Queue<float> Targets = new Queue<float>(5);
		public bool TeleportWhenTooFarAwayFromTarget = true; 

		private float _value;
		private float _velosity = 0;
		private float _currentTarget = 0;

		
		public SmoothInterpolator()
		{

		}
		public SmoothInterpolator(bool teleportWhenTooFarAwayFromTarget = true)
		{
			TeleportWhenTooFarAwayFromTarget = teleportWhenTooFarAwayFromTarget;
		}

		public override float GetCurrentValue(float deltaTime)
		{

			var smoothTime = 0.5f - (0.2f * Targets.Count);
			if(smoothTime <= 0)
			{
				smoothTime = 0.05f;
			}
			_value = Mathf.SmoothDamp(_value, _currentTarget, ref _velosity, smoothTime, 100f, deltaTime);


			if(_value > _currentTarget-0.2f)
			{
				if (Targets.Count == 0)
					return _value;

				_currentTarget = Targets.Dequeue();
			}

			if (TeleportWhenTooFarAwayFromTarget)
			{
				if (Math.Abs(_currentTarget - _value) > 100)
				{
					Teleport(_currentTarget);
				}
			}
			

			return _value;
		}

		public override void Teleport(float value)
		{
			Targets.Clear();
			SetTarget(value);
			_value = value;
			_currentTarget = value;
		}


		public override void SetTarget(float value)
		{
			Targets.Enqueue(value);
		}
	}
}
