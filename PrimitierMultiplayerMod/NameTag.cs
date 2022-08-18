using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PrimitierMultiplayerMod
{
	public class NameTag : MonoBehaviour
	{
		public NameTag(System.IntPtr ptr) : base(ptr) { }

		private TextMeshPro _text;

		public Camera TargetCamera;

		private void Start()
		{
			_text = GetComponent<TextMeshPro>();
			if(TargetCamera == null)
			{
				TargetCamera = Camera.main;
			}
		}


		private void FixedUpdate()
		{
			_text.transform.rotation = Quaternion.LookRotation(transform.position - TargetCamera.transform.position);

		}

	}
}
