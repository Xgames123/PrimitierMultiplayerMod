using Il2CppSystem;
using PrimitierModdingFramework;
using PrimitierModdingFramework.Debugging;
using UnityEngine;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using MelonLoader;
using System.Net;

namespace PrimitierMultiplayerMod
{

	public class Mod : PrimitierMod
    {

		public Client Client = new Client();


		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			base.OnSceneWasLoaded(buildIndex, sceneName);

			var connectSettings = MelonPreferences.GetCategory("ConnectSettings");

			var address = connectSettings.GetEntry<IPAddress>("ServerIp").Value;
			var port = connectSettings.GetEntry<int>("ServerPort").Value;

			Client.Connect(new IPEndPoint(address, port));


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
		
		private void OnExit()
		{

		}


	}
}
