using PrimitierModdingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PrimitierMultiplayerMod
{
	public class Chat : MonoBehaviour
	{
		public Chat(System.IntPtr ptr) : base(ptr) { }

		public TextMeshPro Text;

		private List<string> Lines = new List<string>();

		public static Chat Setup()
		{
			GameObject chatGo = new GameObject("Chat");
			chatGo.transform.parent = null;
			chatGo.transform.localScale = new Vector3(1f, 1f, 1f);
			chatGo.transform.position = new Vector3(0, 1, 0);
			var canvas = chatGo.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.WorldSpace;
			

			var chatComp = chatGo.AddComponent<Chat>();


			GameObject backgroundGo = new GameObject("Background");
			backgroundGo.transform.parent = chatGo.transform;
			backgroundGo.transform.localPosition = Vector3.zero;
			backgroundGo.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			var image = backgroundGo.AddComponent<Image>();
			image.color = new Color(0, 0, 0, 0.3f);



			GameObject textGo = new GameObject("Text");
			textGo.transform.parent = chatGo.transform;
			textGo.transform.localPosition = Vector3.zero;
			textGo.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			chatComp.Text = textGo.AddComponent<TextMeshPro>();
			chatComp.Text.color = Color.white;
			chatComp.Text.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
			chatComp.Text.material.renderQueue = 2900;

			return chatComp;
		}

		private void Start()
		{
		}


		public void AddServerMessage(string message)
		{
			AddMessage("SERVER", message, true);
		}


		public void AddMessage(string sender, string message, bool IsSpecialMessage = false)
		{
			var fullMessage = $"[{sender}] {message}";

			if (IsSpecialMessage)
			{
				PMFLog.Message($"CHAT "+ fullMessage, ConsoleColor.Yellow);
				AddLine($"<color=#FFFF00>{fullMessage}</color>");
			}
			else
			{
				PMFLog.Message($"CHAT "+fullMessage);
				AddLine(fullMessage);
			}
			UpdateText();


		}

		private void AddLine(string line)
		{
			Lines.Add(line);
			Text.text+=line+"\n";
			UpdateText();
		}

		private void UpdateText()
		{

			if (Text.textInfo.lineCount > 23)
			{
				var removeLines = Text.textInfo.lineCount - 23;
				Lines.RemoveRange(0, removeLines);

				Text.text = "";
				foreach (var line in Lines)
				{
					Text.text += line + "\n";
				}

			}


		}



	}
}
