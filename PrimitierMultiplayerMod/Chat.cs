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

		public TMP_Text Text;

		


		public static Chat Setup()
		{
			GameObject chatGo = new GameObject("Chat");
			chatGo.transform.parent = null;
			chatGo.transform.position = new Vector3(0, 1, 0);
			var chatComp = chatGo.AddComponent<Chat>();

			GameObject textGo = new GameObject("Text");
			textGo.transform.parent = chatGo.transform;
			textGo.AddComponent<TMP_Text>();


			chatComp.Text = textGo.GetComponent<TMP_Text>();
			return chatComp;
		}

		private void Start()
		{
			Text.text = "";
		}


		public void AddMessage(string sender, string message, bool IsSpecialMessage = false)
		{
			var fullMessage = $"[{sender}] {message}";

			if (IsSpecialMessage)
			{
				PMFLog.Message($"CHAT "+ fullMessage, ConsoleColor.Yellow);
				WriteColoredTextMessage(fullMessage, Color.yellow);
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


		private void WriteColoredTextMessage(string message, Color32 color)
		{
			WriteTextMessage(message);

			var line = Text.textInfo.lineInfo[Text.textInfo.lineCount - 1];
			for (int i = 0; i < line.characterCount; i++)
			{
				var charIndex = line.firstCharacterIndex + i;
				var meshIndex = Text.textInfo.characterInfo[charIndex].materialReferenceIndex;
				var vertexIndex = Text.textInfo.characterInfo[charIndex].vertexIndex;

				Color32[] vertexColors = Text.textInfo.meshInfo[meshIndex].colors32;
				vertexColors[vertexIndex + 0] = color;
				vertexColors[vertexIndex + 1] = color;
				vertexColors[vertexIndex + 2] = color;
				vertexColors[vertexIndex + 3] = color;
			}

			Text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		}


	}
}
