using PrimitierModdingFramework;
using PrimitierModdingFramework.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PrimitierMultiplayer.ClientLib;

namespace PrimitierMultiplayer.Mod.Components
{


	public class Chat : MonoBehaviour, IChat
	{
		public Chat(IntPtr ptr) : base(ptr) { }

		public TextMeshPro Text;

		private List<string> Lines = new List<string>();



		public static Chat Setup()
		{

			var chatGo = new GameObject("Chat");
			chatGo.transform.parent = PMFHelper.MenuWindowL;
			chatGo.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			chatGo.transform.localRotation = Quaternion.identity;
			chatGo.transform.localPosition = new Vector3(-0.5f, 0.2f, 0f);
			var canvas = chatGo.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.WorldSpace;

			var chatComp = chatGo.AddComponent<Chat>();


			var backgroundGo = new GameObject("Background");
			backgroundGo.transform.parent = chatGo.transform;
			backgroundGo.transform.localRotation = Quaternion.identity;
			backgroundGo.transform.localPosition = Vector3.zero;
			backgroundGo.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			var image = backgroundGo.AddComponent<Image>();
			image.color = new Color(0, 0, 0, 0.3f);



			var textGo = new GameObject("Text");
			textGo.transform.parent = chatGo.transform;
			textGo.transform.localPosition = Vector3.zero;
			textGo.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			textGo.transform.localRotation = Quaternion.identity;
			chatComp.Text = textGo.AddComponent<TextMeshPro>();
			chatComp.Text.color = Color.white;
			chatComp.Text.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
			chatComp.Text.material.renderQueue = 2900;

			return chatComp;
		}

		private void Start()
		{
		}

		public void Clear()
		{
			Lines.Clear();
			Text.text = "";
			UpdateText();
		}

		public void AddMessage(string sender, string message, ChatColor color)
		{

			var fullMessage = $"[{sender}] {message}";

			string line;
			switch (color)
			{
				case ChatColor.NormalText:
					goto default;

				case ChatColor.ServerMessage:
					PMFLog.Message($"CHAT " + fullMessage, ConsoleColor.Yellow);
					line = $"<color=#DDFF00>{fullMessage}</color>";
					break;
				case ChatColor.SystemMessage:
					PMFLog.Message($"CHAT " + fullMessage, ConsoleColor.Cyan);
					line = $"<color=#00BBFF>{fullMessage}</color>";
					break;

				default:
					PMFLog.Message($"CHAT " + fullMessage);
					line = fullMessage;
					break;

			}
			Lines.Add(line);
			Text.text += line + "\n";
			UpdateText();


		}

		public void AddServerMessage(string message)
		{
			AddMessage("SERVER", message, ChatColor.ServerMessage);
		}
		public void AddSystemMessage(string message)
		{
			AddMessage("SYSTEM", message, ChatColor.SystemMessage);
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
