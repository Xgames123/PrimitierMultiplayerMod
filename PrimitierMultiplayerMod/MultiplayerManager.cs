using MelonLoader;
using PrimitierModdingFramework;
using PrimitierMultiplayerMod.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PrimitierMultiplayerMod
{
	public static class MultiplayerManager
	{
		public static Client Client;
		public static MelonPreferences_Entry<string> ServerAddress = null;
		public static MelonPreferences_Entry<int> ServerPort = null;

		public static bool IsInMultiplayerMode = false;

		public static void ConnectToServer()
		{

			var connectSettings = MelonPreferences.CreateCategory("ConnectSettings");

			if(ServerAddress == null)
				ServerAddress = connectSettings.CreateEntry("ServerIp", "localhost");
			if (ServerPort == null)
				ServerPort = connectSettings.CreateEntry<int>("ServerPort", 9543);


			Client = new Client();
			Client.Connect(ServerAddress.Value, ServerPort.Value);
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
			WorldManager.WorldSeed = seed;
			WorldManager.PlayerStartPosition = playerPosition;
			loadingSequence.StartLoading(-1, GameObject.Find("InfoText").GetComponent<TextMeshPro>(), destroyObject, enableObjects);

			JoinGameButton.Destroy();
		}

		public static void ExitGame()
		{
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
			if (Client == null)
				return;

			Client.Stop();
			RemotePlayer.DeleteAllPlayers();
			WorldManager.DestroyAllModChunks();
		}
	}
}
