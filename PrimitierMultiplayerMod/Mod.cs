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
using PrimitierMultiplayerMod.Components;

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

			if (FlyCam.Current != null)
				MultiplayerManager.ConnectToServer();

			PMFLog.Message("You can press F1 to generate all the chunks near 0x 0y");
			PMFLog.Message("You can press F2 to destroy the generated chunks from pressing F2");
			PMFLog.Message("You can press F3 to reconnect to the server");
			PMFLog.Message("You can press F5 for general info");
		}

		public override void OnApplicationStart()
		{
			base.OnApplicationStart();
			
			PMFSystem.EnableSystem<PMFHelper>();

			ClassInjector.RegisterTypeInIl2Cpp<UpdatePacketSender>();
			ClassInjector.RegisterTypeInIl2Cpp<Chat>();
			ClassInjector.RegisterTypeInIl2Cpp<NameTag>();
			ClassInjector.RegisterTypeInIl2Cpp<RemotePlayer>();
			ClassInjector.RegisterTypeInIl2Cpp<NetworkSync>();
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
			MainThreadRunner.RunQueuedTasks();
			

			if (Input.GetKeyUp(KeyCode.F1))
			{
				PMFLog.Message("Generating chunks");
				ChunkManager.GenerateNewPrimitierChunk(new Vector2Int(-1, -1));
				ChunkManager.GenerateNewPrimitierChunk(new Vector2Int(0, -1));
				ChunkManager.GenerateNewPrimitierChunk(new Vector2Int(1, -1));

				ChunkManager.GenerateNewPrimitierChunk(new Vector2Int(-1, 0));
				ChunkManager.GenerateNewPrimitierChunk(new Vector2Int(0, 0));
				ChunkManager.GenerateNewPrimitierChunk(new Vector2Int(1, 0));

				ChunkManager.GenerateNewPrimitierChunk(new Vector2Int(-1, 1));
				ChunkManager.GenerateNewPrimitierChunk(new Vector2Int(0, 1));
				ChunkManager.GenerateNewPrimitierChunk(new Vector2Int(1, 1));
			}
			if (Input.GetKeyUp(KeyCode.F2))
			{
				PMFLog.Message("Destroying chunks");
				var chunks = new Il2CppSystem.Collections.Generic.List<Vector2Int>();
				chunks.Add(new Vector2Int(-1, -1));
				chunks.Add(new Vector2Int(0, -1));
				chunks.Add(new Vector2Int(1, -1));

				chunks.Add(new Vector2Int(-1, 0));
				chunks.Add(new Vector2Int(0, 0));
				chunks.Add(new Vector2Int(1, 0));

				chunks.Add(new Vector2Int(-1, 1));
				chunks.Add(new Vector2Int(0, 1));
				chunks.Add(new Vector2Int(1, 1));
				ChunkManager.DestroyPrimitierChunks(chunks);
			}
			if (Input.GetKeyUp(KeyCode.F3))
			{
				MultiplayerManager.Stop();
				MultiplayerManager.ConnectToServer();
			}
			if (Input.GetKeyUp(KeyCode.F4))
			{
				ChunkManager.UpdateCube(new PrimitierServer.Shared.NetworkCube() { Id = 2, Position = new System.Numerics.Vector3(0, 0, 0), Size = new System.Numerics.Vector3(5, 5, 5), Substance = 0, Rotation = new System.Numerics.Quaternion(0, 0, 0, 0) });
			}
			//ChunkManager.UpdateModChunk(new PrimitierServer.Shared.NetworkChunk() { Cubes = new System.Collections.Generic.List<PrimitierServer.Shared.NetworkCube>() { new PrimitierServer.Shared.NetworkCube() { Id = 1, Position = new System.Numerics.Vector3(0, 0, 0), Size = new System.Numerics.Vector3(5, 5, 5), Substance = 0, Rotation = new System.Numerics.Quaternion(0, 0, 0, 0) } } });

			if (Input.GetKeyUp(KeyCode.F5))
			{
				var playerPos = Camera.main.transform.position;
				var playerChunkPos = CubeGenerator.WorldToChunkPos(playerPos);

				PMFLog.Message($"world seed: {TerrainGenerator.worldSeed}");
				PMFLog.Message($"player pos X: {playerPos.x}, Y: {playerPos.y}, Z: {playerPos.z}");
				PMFLog.Message($"player chunk pos X: {playerChunkPos.x}, Y: {playerChunkPos.y}");
				
			}

		}
		

	}
}
