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
	public static class ChunkManager
	{
		public static bool AllowGenerateNextChunk = false;
		public static bool AllowDestroyNextChunk = false;
		public static int WorldSeed = -1;

		public static void UpdateModChunks(IEnumerable<NetworkChunk> chunks)
		{
			foreach (var chunk in chunks)
			{
				UpdateModChunk(chunk);
			}

		}

		public static void UpdateModChunk(NetworkChunk chunk)
		{
			if(chunk.ChunkType != NetworkChunkType.Normal)
			{
				return;
			}

			foreach (var cube in chunk.Cubes)
			{
				UpdateCube(cube);
			}
		}
		public static void DestroyModChunk(System.Numerics.Vector2 chunkPos)
		{
			var syncs = NetworkSync.GetSyncsInChunk(chunkPos);
			foreach (var sync in syncs)
			{
				sync.DestroyCube();
			}
		}
		public static void DestroyAllModChunks()
		{
			foreach (var chunk in NetworkSync.NetworkSyncChunk.Keys)
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
