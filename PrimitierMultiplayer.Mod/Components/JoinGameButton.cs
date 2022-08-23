using MelonLoader;
using PrimitierModdingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace PrimitierMultiplayer.Mod.Components
{
	public class JoinGameButton : MonoBehaviour
	{
		public JoinGameButton(IntPtr ptr) : base(ptr) { }

		public static void Destroy()
		{
			Destroy(GameObject.Find("JoinGameButton"));

		}


		public static JoinGameButton Create()
		{
			var joinGameButtonGo = new GameObject("JoinGameButton");
			//Setting the parent to TitleMenu (the other buttons) breaks Primitier for some reason
			joinGameButtonGo.transform.parent = SafeTransformFind("TitleSpace");
			joinGameButtonGo.transform.localPosition = new Vector3(-1.5f, 1.1f, 1.9f);
			joinGameButtonGo.transform.localRotation = Quaternion.identity;
			joinGameButtonGo.transform.localScale = new Vector3(1, 1, 1);
			var joinGameButton = joinGameButtonGo.AddComponent<JoinGameButton>();


			var cubeGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cubeGo.transform.parent = joinGameButtonGo.transform;
			cubeGo.transform.localPosition = Vector3.zero;
			cubeGo.transform.localRotation = Quaternion.identity;
			cubeGo.transform.localScale = new Vector3(1f, 0.3f, 0.05f);

			var textGo = new GameObject("Text");
			textGo.transform.parent = joinGameButtonGo.transform;
			textGo.transform.localPosition = new Vector3(0, 0, -0.03f);
			textGo.transform.localRotation = Quaternion.identity;
			textGo.transform.localScale = new Vector3(1f, 1f, 1f);
			var tmp = textGo.AddComponent<TextMeshPro>();
			tmp.autoSizeTextContainer = true;
			tmp.font = PMFHelper.PrimitierDefaultFont;
			tmp.text = "CONNECT TO SERVER";
			tmp.color = Color.black;
			tmp.fontSize = 0.8f;

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
