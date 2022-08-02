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
using PrimitierMultiplayerMod.ComponentDumpers;

namespace PrimitierMultiplayerMod
{

	public class Mod : PrimitierMod
    {


		public static Chat Chat;

		
		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			base.OnSceneWasLoaded(buildIndex, sceneName);

			PlayerInfo.Load();
			UpdatePacketSender.Setup();

			
			Chat = Chat.Setup();

			JoinGameButton.Create();
		}



		public override void OnRealyLateStart()
		{
			base.OnRealyLateStart();
			//HierarchyXmlDumper.DumpSceneToFile();

			MultiplayerManager.ConnectToServer();
		}

		public override void OnApplicationStart()
		{
			base.OnApplicationStart();
			
			PMFSystem.EnableSystem<PMFHelper>();

			ClassInjector.RegisterTypeInIl2Cpp<UpdatePacketSender>();
			ClassInjector.RegisterTypeInIl2Cpp<Chat>();
			ClassInjector.RegisterTypeInIl2Cpp<NameTag>();
			ClassInjector.RegisterTypeInIl2Cpp<RemotePlayer>();
			ClassInjector.RegisterTypeInIl2CppWithInterfaces<JoinGameButton>(typeof(IButton));
			HierarchyXmlDumper.DefaultDumperList.Add(new TextMeshProComponentDumper());
			HierarchyXmlDumper.DefaultDumperList.Add(new StartButtonComponentDumper());
		}
		public override void OnApplicationQuit()
		{
			base.OnApplicationQuit();
			MultiplayerManager.Stop();
			
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			
		}

		public override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			MultiplayerManager.UpdateClient();

			if(Input.GetKeyUp(KeyCode.F1))
			{
				var chunks = new Il2CppSystem.Collections.Generic.List<Vector2Int>();
				chunks.Add(new Vector2Int(0, 0));
				CubeGenerator.DestroyChunks(chunks);
				
				
			}
			if (Input.GetKeyUp(KeyCode.F2))
			{
				CubeGenerator.GenerateChunk(new Vector2Int(0, 0));
			}

		}
		

	}
}
