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

		private void Start()
		{
			_text = GetComponent<TextMeshPro>();
		}


		private void FixedUpdate()
		{
			_text.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

		}

	}
}
