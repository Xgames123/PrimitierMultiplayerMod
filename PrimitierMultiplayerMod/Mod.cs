using Il2CppSystem;
using PrimitierModdingFramework;
using PrimitierModdingFramework.Debugging;
using UnityEngine;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using MelonLoader;
using System.Net;
using LiteNetLib;

using PrimitierModdingFramework.SubstanceModding;
namespace PrimitierMultiplayerMod
{

	public class Mod : PrimitierMod
    {

		public Client Client;

		public string ServerAddress;
		public int ServerPort;

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			base.OnSceneWasLoaded(buildIndex, sceneName);

			//var param = CustomSubstanceSystem.CreateCustomSubstance(Substance.Ice);
			//param.physicMaterial = SubstanceManager.

			HierarchyXmlDumper.DumpSceneToFile();

			PlayerInfo.Load();

			var connectSettings = MelonPreferences.CreateCategory("ConnectSettings");

			ServerAddress = connectSettings.CreateEntry("ServerIp", "localhost").Value;
			ServerPort = connectSettings.CreateEntry<int>("ServerPort", 9543).Value;

			Connect();
			PMFLog.Message("Connecting to server");
		}
		private void Connect()
		{
			Client = new Client();
			Client.Connect(ServerAddress, ServerPort);

		}


		public override void OnRealyLateStart()
		{
			base.OnRealyLateStart();
			
		}

		public override void OnApplicationStart()
		{
			base.OnApplicationStart();
			//PMFSystem.EnableSystem<PMFHelper>();

			
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

			if (Input.GetKeyUp(KeyCode.Return))
			{
				Client.Stop();
				Connect();
				PMFLog.Message("Reconnecting to server...");

			}


		}

		public override void OnFixedUpdate()
		{
			base.OnFixedUpdate();
			Client.Update();
		}
		

	}
}
