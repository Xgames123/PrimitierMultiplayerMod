using PrimitierModdingFramework;
using PrimitierMultiplayer.Mod.Components;
using PrimitierMultiplayer.Shared;
using PrimitierMultiplayer.Shared.Models;
using PrimitierMultiplayer.Shared.Models.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimitierMultiplayer.Mod
{
	public static class WorldManager
	{
		public static bool AllowGenerateNextChunk = false;
		public static bool AllowDestroyNextChunk = false;
		public static int WorldSeed = -1;

		public static UnityEngine.Vector3 PlayerStartPosition;

		public static Dictionary<System.Numerics.Vector2, RuntimeChunk> VisibleChunks = new Dictionary<System.Numerics.Vector2, RuntimeChunk>();


		public static RuntimeChunk GetVisibleChunk(System.Numerics.Vector2 chunkPos)
		{
			if (VisibleChunks.TryGetValue(chunkPos, out var chunk))
			{
				return chunk;
			}

			return null;
		}






		public static void UpdateModChunk(NetworkChunkPositionPair chunkPosPair)
		{
			var chunk = chunkPosPair.Chunk;
			var chunkPos = chunkPosPair.Position;


			//Skip when broken
			if (chunk.ChunkType == NetworkChunkType.Broken)
			{
				return;
			}

			bool wasInCache = true;
			var runtimeChunk = GetVisibleChunk(chunkPos);
			if (runtimeChunk == null)
			{
				wasInCache = false;
				runtimeChunk = CreateEmptyModChunk(chunkPosPair);
			}
			if (!wasInCache)
			{
				//Sync chunk from server
				SyncChunkFromServer(chunkPosPair);
				return;
			}


			if (chunk.Owner == MultiplayerManager.LocalId)
			{
				//Skip chunk because we own it
				return;
			}
			else
			{
				//Sync chunk from server
				SyncChunkFromServer(chunkPosPair);


			}
			
			
		
		}
		private static void SyncChunkFromServer(NetworkChunkPositionPair chunkPosPair)
		{
			PMFLog.Message($"Syncing X: {chunkPosPair.Position.X}, Y: {chunkPosPair.Position.Y} from server");
			PMFLog.Message($"chunk contains {chunkPosPair.Chunk.Cubes.Count} cubes");

			var chunk = chunkPosPair.Chunk;
			var chunkPos = chunkPosPair.Position;

			foreach (var cube in chunk.Cubes)
			{
				UpdateCube(cube);
			}


			//Remove old network syncs
			foreach (var netSyncId in GetVisibleChunk(chunkPos).NetworkSyncs)
			{
				var cubeSync = CubeSync.GetById(netSyncId);
				if (!Contains(chunk.Cubes, netSyncId))
				{
					cubeSync.DestroyCube();

				}

			}
		}


		private static bool Contains(IEnumerable<NetworkCube> cubes, uint id)
		{
			foreach (var cube in cubes)
			{
				if (cube.Id == id)
				{
					return true;
				}

			}
			return false;
		}

		private static RuntimeChunk CreateEmptyModChunk(NetworkChunkPositionPair chunkPair)
		{
			var chunk = chunkPair.Chunk;

			var runtimeChunk = new RuntimeChunk() { Owner = chunk.Owner };
			VisibleChunks.Add(chunkPair.Position, runtimeChunk);

			return runtimeChunk;
		}


		public static void DestroyModChunk(System.Numerics.Vector2 chunkPos)
		{
			var chunk = GetVisibleChunk(chunkPos);
			foreach (var syncId in chunk.NetworkSyncs)
			{
				var sync = CubeSync.GetById(syncId);
				if(sync != null)
				{
					sync.DestroyCube();
				}
					
			}

			VisibleChunks.Remove(chunkPos);
		}
		public static void DestroyAllModChunks()
		{

			foreach (var chunk in VisibleChunks.Keys.ToArray())
			{
				DestroyModChunk(chunk);
			}
		}


		public static void CreateCube(NetworkCube cube)
		{
			var primCube = CubeGenerator.GenerateCube(cube.Position.ToUnity(), cube.Size.ToUnity(), (Substance)cube.Substance);
			var networkSync = primCube.AddComponent<CubeSync>();
			networkSync.Id = cube.Id;
			CubeSync.Register(networkSync);
			//PMFLog.Message("Cube created");
		}
		public static void UpdateCube(NetworkCube cube)
		{
			//PMFLog.Message($"Id: {cube.Id} Position: {cube.Position} Rotation: {cube.Rotation} Size: {cube.Size} Substance: {cube.Substance}");

			if (CubeSync.CubeSyncList.TryGetValue(cube.Id, out var sync))
			{
				sync.UpdateFromServer(cube);
			}
			else
			{
				CreateCube(cube);
				
			}
		}


		public static void DestroyPrimitierChunks(Il2CppSystem.Collections.Generic.List<Vector2Int> chunkPositions)
		{
			AllowDestroyNextChunk = true;
			CubeGenerator.DestroyChunks(chunkPositions);
			AllowDestroyNextChunk = false;
		}

		public static void GenerateNewPrimitierChunk(UnityEngine.Vector2Int position)
		{
			AllowGenerateNextChunk = true;
			CubeGenerator.GenerateNewChunk(position);

			AllowGenerateNextChunk = false;
		}
	}
}
