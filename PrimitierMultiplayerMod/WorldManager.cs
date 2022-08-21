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
		public static List<System.Numerics.Vector2> OwnedChunks = new List<System.Numerics.Vector2>();


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
			OwnedChunks.Clear();
			foreach (var chunk in chunks)
			{
				UpdateModChunk(chunk);
			}

			foreach (var chunkPos in Chunks.Keys.ToArray())
			{
				if (!Contains(chunks, chunkPos))
				{
					DestroyModChunk(chunkPos);
				}

			}

		}
		private static bool Contains(IEnumerable<NetworkChunkPositionPair> container, System.Numerics.Vector2 position)
		{
			foreach (var item in container)
			{
				if (item.Position == position)
					return true;

			}
			return false;
		}


		public static void UpdateModChunk(NetworkChunkPositionPair chunkPosPair)
		{
			var chunk = chunkPosPair.Chunk;
			var chunkPos = chunkPosPair.Position;

			//Generate chunk when empty and owned
			if (chunk.ChunkType != NetworkChunkType.Normal)
			{
				return;
			}

			var runtimeChunk = GetChunk(chunkPos);
			if (runtimeChunk == null)
			{
				CreateModChunk(chunkPosPair);
			}

			if (chunk.Owner == MultiplayerManager.Client.LocalId)
			{
				//Skip chunk because we own it

				OwnedChunks.Add(chunkPos);
				return;
			}
			else
			{
				//Sync chunk from server

				foreach (var cube in chunk.Cubes)
				{
					UpdateCube(cube, chunkPos);
				}


				//Remove old network syncs
				foreach (var netSyncId in Chunks[chunkPos].NetworkSyncs)
				{
					if(!Contains(chunk.Cubes, netSyncId))
					{
						NetworkSync.GetById(netSyncId).DestroyCube();

					}

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

		private static void CreateModChunk(NetworkChunkPositionPair chunkPair)
		{
			var chunk = chunkPair.Chunk;

			Chunks.Add(chunkPair.Position, new RuntimeChunk() { Owner = chunk.Owner });

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
			Chunks.Remove(chunkPos);
		}
		public static void DestroyAllModChunks()
		{
			foreach (var chunk in Chunks.Keys)
			{
				DestroyModChunk(chunk);
			}
		}


		public static void CreateCube(NetworkCube cube, System.Numerics.Vector2 chunkPos)
		{
			var primCube = CubeGenerator.GenerateCube(cube.Position.ToUnity(), cube.Size.ToUnity(), (Substance)cube.Substance);
			var networkSync = primCube.AddComponent<NetworkSync>();
			networkSync.Id = cube.Id;
			NetworkSync.Register(networkSync, chunkPos);
			//PMFLog.Message("Cube created");
		}
		public static void UpdateCube(NetworkCube cube, System.Numerics.Vector2 chunkPos)
		{
			//PMFLog.Message($"Id: {cube.Id} Position: {cube.Position} Rotation: {cube.Rotation} Size: {cube.Size} Substance: {cube.Substance}");

			if (NetworkSync.NetworkSyncList.TryGetValue(cube.Id, out var sync))
			{

				sync.UpdateSync(cube, chunkPos);
			}
			else
			{
				CreateCube(cube, chunkPos);
				
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
