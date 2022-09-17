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
using PrimitierMultiplayer.Mod.Components;
using System.Linq;
using PrimitierMultiplayer.Shared;

namespace PrimitierMultiplayer.Mod
{

	public class Mod : PrimitierMod
    {
		public static System.Version ModVersion;
		
		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			base.OnSceneWasLoaded(buildIndex, sceneName);

			MultiplayerManager.Init();

			PlayerInfo.Load();

			JoinGameButton.Create();

			//Validate terrain settings
			if (TerrainMeshGenerator.tileLength != 4 || CubeGenerator.chunkTileCount != 4)
			{
				PMFLog.Error("Terrain generator settings are changed. Trying to reset settings");
				TerrainMeshGenerator.tileLength = 4;
				CubeGenerator.chunkTileCount = 4;
				
			}

		}



		public override void OnRealyLateStart()
		{
			base.OnRealyLateStart();
			//HierarchyXmlDumper.DumpSceneToFile();

			if (FlyCam.Current != null)
				MultiplayerManager.Instance.ConnectToServer();

			PMFLog.Message("You can press F2 to disconnect from the server");
			PMFLog.Message("You can press F3 to reconnect to the server");
			PMFLog.Message("You can press F4 to show/hide chunk bounds");
			PMFLog.Message("You can press F5 for general info");
			PMFLog.Message("You can press F6 to dump scene");
			PMFLog.Message(" ");
			PMFLog.Message("You can press F9 to generate all the chunks near 0x 0y");
			PMFLog.Message("You can press F10 to destroy the generated chunks from pressing F9");

		}

		public override void OnApplicationStart()
		{
			base.OnApplicationStart();
			
			PMFSystem.EnableSystem<PMFHelper>();

			ClassInjector.RegisterTypeInIl2Cpp<Chat>();
			ClassInjector.RegisterTypeInIl2Cpp<NameTag>();
			ClassInjector.RegisterTypeInIl2Cpp<RemotePlayer>();
			ClassInjector.RegisterTypeInIl2Cpp<CubeSync>();
			ClassInjector.RegisterTypeInIl2Cpp<ChunkBoundViewer>();
			ClassInjector.RegisterTypeInIl2CppWithInterfaces<JoinGameButton>(typeof(IButton));

			ModVersion = new System.Version(Info.Version);

			
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

			MultiplayerManager.Instance.UpdateClient();
			MainThreadRunner.RunQueuedTasks();
			
			if (Input.GetKeyUp(KeyCode.F2))
			{
				MultiplayerManager.Stop();
			}

			if (Input.GetKeyUp(KeyCode.F3))
			{
				MultiplayerManager.Stop();
				MultiplayerManager.Instance.ConnectToServer();
			}
			if (Input.GetKeyUp(KeyCode.F4))
			{
				if(ChunkBoundViewer.IsCreated)
					ChunkBoundViewer.Remove();
				else
					ChunkBoundViewer.Create();
			}
			if (Input.GetKeyUp(KeyCode.F5))
			{
				var playerPos = Camera.main.transform.position.ToNumerics();
				var playerChunkPos = ChunkMath.WorldToChunkPos(playerPos);

				PMFLog.Message("");
				PMFLog.Message("GENERAL DATA:");
				PMFLog.Message($"	World seed: {TerrainGenerator.worldSeed}");
				PMFLog.Message($"	Player pos X: {playerPos.X}, Y: {playerPos.Y}, Z: {playerPos.Z}");
				PMFLog.Message($"	Player chunk pos X: {playerChunkPos.X}, Y: {playerChunkPos.Y}");
				PMFLog.Message($"	Remote player count: {RemotePlayer.RemotePlayers.Count}");
				PMFLog.Message($"	Synced object count: {CubeSync.CubeSyncList.Count}");
				PMFLog.Message($"	Chunk size: {TerrainMeshGenerator.tileLength}");
				PMFLog.Message($"	Tiles per chunk: {CubeGenerator.chunkTileCount}");
				PMFLog.Message($"	AreaPosOffset: X: {TerrainMeshGenerator.areaPosOffset.x}, Y: {TerrainMeshGenerator.areaPosOffset.y}");
				PMFLog.Message($"");
				PMFLog.Message($"	OWNED CHUNKS:");
				foreach (var chunkPos in WorldManager.VisibleChunks.Keys)
				{
					var chunk = WorldManager.GetVisibleChunk(chunkPos);
					PMFLog.Message($"		X: {chunkPos.X}, Y: {chunkPos.Y} Owned: {chunk.Owner == MultiplayerManager.Instance.LocalId}");
					
					foreach (var netSync in chunk.NetworkSyncs)
					{
						PMFLog.Message($"			Id: {netSync}");
					}
					
				}
			}


			if (Input.GetKeyUp(KeyCode.F6))
			{
				HierarchyXmlDumper.DumpSceneToFile();

				PMFLog.Message("Layer mask dump");

				for (int i = 0; i < 31; i++)
				{
					var layer = LayerMask.LayerToName(i);
					if (!string.IsNullOrWhiteSpace(layer))
					{
						PMFLog.Message($"{i} : {layer}");
					}
				}

			}

			if (Input.GetKeyUp(KeyCode.F9))
			{
				PMFLog.Message("Generating chunks");
				WorldManager.GenerateNewPrimitierChunk(new Vector2Int(-1, -1));
				WorldManager.GenerateNewPrimitierChunk(new Vector2Int(0, -1));
				WorldManager.GenerateNewPrimitierChunk(new Vector2Int(1, -1));

				WorldManager.GenerateNewPrimitierChunk(new Vector2Int(-1, 0));
				WorldManager.GenerateNewPrimitierChunk(new Vector2Int(0, 0));
				WorldManager.GenerateNewPrimitierChunk(new Vector2Int(1, 0));

				WorldManager.GenerateNewPrimitierChunk(new Vector2Int(-1, 1));
				WorldManager.GenerateNewPrimitierChunk(new Vector2Int(0, 1));
				WorldManager.GenerateNewPrimitierChunk(new Vector2Int(1, 1));
			}
			if (Input.GetKeyUp(KeyCode.F10))
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
				WorldManager.DestroyPrimitierChunks(chunks);
			}
		}
		

	}
}
