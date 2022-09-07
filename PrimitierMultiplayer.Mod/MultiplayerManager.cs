using MelonLoader;
using PrimitierModdingFramework;
using PrimitierMultiplayer.Mod.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PrimitierMultiplayer.Mod
{
	public static class MultiplayerManager
	{
		public static Client Client;
		public static MelonPreferences_Entry<string> ServerAddress = null;
		public static MelonPreferences_Entry<int> ServerPort = null;

		public static int LocalId = -1;

		public static bool IsInMultiplayerMode = false;

		public static void Init()
		{
			Client = new Client();
			Client.OnDisconnectFromServer += Client_OnDisconnectFromServer;
			Client.Start();
		}


		public static void ConnectToServer()
		{

			var connectSettings = MelonPreferences.CreateCategory("ConnectSettings");

			if(ServerAddress == null)
				ServerAddress = connectSettings.CreateEntry("ServerIp", "localhost");
			if (ServerPort == null)
				ServerPort = connectSettings.CreateEntry<int>("ServerPort", 9543);

			Client.Start();
			Client.Connect(ServerAddress.Value, ServerPort.Value);
		}

		private static void Client_OnDisconnectFromServer(LiteNetLib.NetPeer obj)
		{
			Stop();
		}

		public static void EnterGame(int seed, Vector3 playerPosition)
		{
			IsInMultiplayerMode = true;

			var loadingSequence = GameObject.FindObjectOfType<LoadingSequence>();

			var destroyObject = new UnhollowerBaseLib.Il2CppReferenceArray<GameObject>(0);
			var titleSpace = GameObject.Find("TitleSpace");
			if (titleSpace != null)
			{
				destroyObject = new UnhollowerBaseLib.Il2CppReferenceArray<GameObject>(1);
				destroyObject[0] = titleSpace;
			}

			var enableObjects = new UnhollowerBaseLib.Il2CppReferenceArray<GameObject>(0);


			PMFHelper.CameraRig.position = Vector3.zero;
			Camera.main.transform.position = Vector3.zero;
			TerrainMeshGenerator.areaPosOffset = Vector2Int.zero;
			

			WorldManager.WorldSeed = seed;
			WorldManager.PlayerStartPosition = playerPosition;

			
			var infoTextGo = GameObject.Find("InfoText");
			TextMeshPro infoText = null;
			if (infoTextGo != null)
			{
				infoText = infoTextGo.GetComponent<TextMeshPro>();
			}


			loadingSequence.StartLoading(-1, infoText, destroyObject, enableObjects);

			JoinGameButton.Destroy();
		}

		public static void ExitGame()
		{
			Mod.Chat.Clear();
			IsInMultiplayerMode = false;
		}


		public static void UpdateClient()
		{
			if (Client == null)
				return;
			Client.Update();
		}

		public static void Stop()
		{
			ExitGame();
			
			if(Client.IsRunning)
				Client.Stop();
			RemotePlayer.DeleteAllPlayers();
			WorldManager.DestroyAllModChunks();
		}
	}
}
