using PrimitierModdingFramework;
using PrimitierMultiplayerMod.Components;
using PrimitierServer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimitierMultiplayerMod
{
	public static class WorldManager
	{
		public static bool AllowGenerateNextChunk = false;
		public static bool AllowDestroyNextChunk = false;
		public static int WorldSeed = -1;

		public static UnityEngine.Vector3 PlayerStartPosition;

		public static Dictionary<System.Numerics.Vector2, RuntimeChunk> Chunks = new Dictionary<System.Numerics.Vector2, RuntimeChunk>();


		public static RuntimeChunk GetChunk(System.Numerics.Vector2 chunkPos)
		{
			if (Chunks.TryGetValue(chunkPos, out var chunk))
			{
				return chunk;
			}

			return null;
		}



		public static void UpdateModChunks(IEnumerable<NetworkChunkPositionPair> chunks)
		{
			foreach (var chunk in chunks)
			{
				UpdateModChunk(chunk);
			}

		}

		public static void UpdateModChunk(NetworkChunkPositionPair chunkPosPair)
		{
			var chunk = chunkPosPair.Chunk;
			var chunkPos = chunkPosPair.Position;

			if(chunk.ChunkType != NetworkChunkType.Normal)
			{
				return;
			}

			var cachedChunk = GetChunk(chunkPos);
			if(cachedChunk == null)
			{

			}
			
			foreach (var cube in chunk.Cubes)
			{
				UpdateCube(cube);
			}
		}
		public static void DestroyModChunk(System.Numerics.Vector2 chunkPos)
		{
			var chunk = GetChunk(chunkPos);
			foreach (var syncId in chunk.NetworkSyncs)
			{
				var sync = NetworkSync.GetById(syncId);
				if(sync != null)
					sync.DestroyCube();
			}
		}
		public static void DestroyAllModChunks()
		{
			foreach (var chunk in Chunks.Keys)
			{
				DestroyModChunk(chunk);
			}
		}


		public static void CreateCube(NetworkCube cube)
		{
			var primCube = CubeGenerator.GenerateCube(cube.Position.ToUnity(), cube.Size.ToUnity(), (Substance)cube.Substance);
			var networkSync = primCube.AddComponent<NetworkSync>();
			networkSync.Id = cube.Id;
			NetworkSync.Register(networkSync);
			//PMFLog.Message("Cube created");
		}
		public static void UpdateCube(NetworkCube cube)
		{
			//PMFLog.Message($"Id: {cube.Id} Position: {cube.Position} Rotation: {cube.Rotation} Size: {cube.Size} Substance: {cube.Substance}");

			if (NetworkSync.NetworkSyncList.TryGetValue(cube.Id, out var sync))
			{
				sync.UpdateSync(cube);
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
