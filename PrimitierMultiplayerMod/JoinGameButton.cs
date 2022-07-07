using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimitierMultiplayerMod
{
	public class JoinGameButton : MonoBehaviour
	{
		public JoinGameButton(System.IntPtr ptr) : base(ptr) { }


		public static JoinGameButton Create()
		{
			var joinGameButtonGo = new GameObject("JoinGameButton");
			joinGameButtonGo.transform.parent = SafeTransformFind("TitleMenu");
			joinGameButtonGo.transform.localPosition = new Vector3(-1.5f, 1.1f, 1.9f);
			joinGameButtonGo.transform.localRotation = Quaternion.identity;
			joinGameButtonGo.transform.localScale = new Vector3(1, 1, 1);
			var joinGameButton = joinGameButtonGo.AddComponent<JoinGameButton>();


			var cubeGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cubeGo.transform.parent = joinGameButtonGo.transform;
			cubeGo.transform.localPosition = Vector3.zero;
			cubeGo.transform.localRotation = Quaternion.identity;
			cubeGo.transform.localScale = new Vector3(0.7f, 0.3f, 0.1f);

			var textGo = new GameObject("Text");
			textGo.transform.parent = joinGameButtonGo.transform;
			textGo.transform.localPosition = Vector3.zero;
			textGo.transform.localRotation = Quaternion.identity;
			textGo.transform.localScale = new Vector3(0.7f, 0.3f, 0.1f);
			var tmp = textGo.AddComponent<TMPro.TextMeshPro>();
			tmp.text = "Connect to server";
			tmp.autoSizeTextContainer = true;
			tmp.fontSize = 1;

			return joinGameButton;
		}

		private static Transform SafeTransformFind(string name)
		{
			var go = GameObject.Find(name);
			if (go == null)
				return null;
			return go.transform;
		}


		public void OnPress()
		{
			MultiplayerManager.ConnectToServer();

		}


	}
}
