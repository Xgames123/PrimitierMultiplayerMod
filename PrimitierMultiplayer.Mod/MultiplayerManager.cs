using MelonLoader;
using PrimitierModdingFramework;
using PrimitierMultiplayer.ClientLib;
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
	public class MultiplayerManager : MultiplayerManagerBase
	{

		public static MelonPreferences_Entry<string> ServerAddress = null;
		public static MelonPreferences_Entry<int> ServerPort = null;


		public MultiplayerManager(IChat chat) : base(chat)
		{

			
		}

		public static void Init()
		{
			var chat = PrimitierMultiplayer.Mod.Components.Chat.Setup();

			Instance = new MultiplayerManager(chat);
		}


		public override void ConnectToServer()
		{
			var connectSettings = MelonPreferences.CreateCategory("ConnectSettings");

			if (ServerAddress == null)
				ServerAddress = connectSettings.CreateEntry("ServerIp", "localhost");
			if (ServerPort == null)
				ServerPort = connectSettings.CreateEntry<int>("ServerPort", 9543);
			base.ConnectToServer(ServerAddress.Value, ServerPort.Value);
		}

		protected override void CreateWorld(int seed, System.Numerics.Vector3 playerPosition)
		{
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
			WorldManager.PlayerStartPosition = playerPosition.ToUnity();


			var infoTextGo = GameObject.Find("InfoText");
			TextMeshPro infoText = null;
			if (infoTextGo != null)
			{
				infoText = infoTextGo.GetComponent<TextMeshPro>();
			}


			loadingSequence.StartLoading(-1, infoText, destroyObject, enableObjects);

			JoinGameButton.Destroy();
		}


		protected override void DestroyWorld()
		{
			RemotePlayer.DeleteAllPlayers();
		}
	}
}
