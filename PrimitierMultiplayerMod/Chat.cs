using PrimitierModdingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PrimitierMultiplayerMod
{
	public class Chat : MonoBehaviour
	{
		public Chat(System.IntPtr ptr) : base(ptr) { }

		public TextMeshPro Text;

		


		public static Chat Setup()
		{
			GameObject chatGo = new GameObject("Chat");
			chatGo.transform.parent = null;
			chatGo.transform.position = new Vector3(0, 1, 0);
			var chatComp = chatGo.AddComponent<Chat>();

			GameObject textGo = new GameObject("Text");
			textGo.transform.parent = chatGo.transform;
			textGo.transform.localPosition = new Vector3(0, 0, 0);
			textGo.transform.localScale = new Vector3(1, 1, 1);
			chatComp.Text = textGo.AddComponent<TextMeshPro>();
			chatComp.Text.fontSize = 0.5f;
			chatComp.Text.color = Color.gray;

			return chatComp;
		}

		private void Start()
		{
		}


		public void AddMessage(string sender, string message, bool IsSpecialMessage = false)
		{
			var fullMessage = $"[{sender}] {message}";

			if (IsSpecialMessage)
			{
				PMFLog.Message($"CHAT "+ fullMessage, ConsoleColor.Yellow);
				WriteTextMessage($"<color=#FFFF00>{fullMessage}</color>");
			}
			else
			{
				PMFLog.Message($"CHAT "+fullMessage);
				WriteTextMessage(fullMessage);
			}

			


		}

		private void WriteTextMessage(string message)
		{
			Text.text += message + "\n";
		}


		//private void WriteColoredTextMessage(string message, Color32 color)
		//{
		//	WriteTextMessage(message);

		//	var line = Text.textInfo.lineInfo[Text.textInfo.lineCount - 1];
		//	for (int i = 0; i < line.characterCount; i++)
		//	{
		//		var charIndex = line.firstCharacterIndex + i;
		//		var meshIndex = Text.textInfo.characterInfo[charIndex].materialReferenceIndex;
		//		var vertexIndex = Text.textInfo.characterInfo[charIndex].vertexIndex;

		//		Color32[] vertexColors = Text.textInfo.meshInfo[meshIndex].colors32;
		//		vertexColors[vertexIndex + 0] = color;
		//		vertexColors[vertexIndex + 1] = color;
		//		vertexColors[vertexIndex + 2] = color;
		//		vertexColors[vertexIndex + 3] = color;
		//	}

		//	Text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		//}


	}
}
