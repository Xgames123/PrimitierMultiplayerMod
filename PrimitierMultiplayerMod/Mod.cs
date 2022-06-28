using Il2CppSystem;
using PrimitierModdingFramework;
using PrimitierModdingFramework.Debugging;
using UnityEngine;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using MelonLoader;
using System.Net;
using LiteNetLib;

namespace PrimitierMultiplayerMod
{

	public class Mod : PrimitierMod
    {

		public Client Client;

		private static NetManager client;

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			base.OnSceneWasLoaded(buildIndex, sceneName);

			PlayerInfo.Load();

			var connectSettings = MelonPreferences.CreateCategory("ConnectSettings");

			var address = connectSettings.CreateEntry("ServerIp", "localhost").Value;
			var port = connectSettings.CreateEntry<int>("ServerPort", 9586).Value;

			Client = new Client();
			Client.Connect(address, port);
			PMFLog.Message("Connecting to server");



		}


		public override void OnRealyLateStart()
		{
			base.OnRealyLateStart();
			
		}

		public override void OnApplicationStart()
		{
			base.OnApplicationStart();
			PMFSystem.EnableSystem<PMFHelper>();

			
		}
		public override void OnApplicationQuit()
		{
			base.OnApplicationQuit();
			PMFLog.Message("Stopping client");
			Client.Stop();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
		}

		public override void OnFixedUpdate()
		{
			base.OnFixedUpdate();
			Client.Update();
		}
		

	}
}
