using PrimitierMultiplayerMod.Components;
using PrimitierServer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod
{
	public static class ChunkManager
	{
		public static bool GenerateNextChunk = false;
		public static int WorldSeed = -1;


		public static void GenerateChunk(NetworkChunk chunk)
		{
			
			foreach (var cube in chunk.Cubes)
			{
				UpdateCube(cube);
			}
		}

		public static void CreateCube(NetworkCube cube)
		{
			var primCube = CubeGenerator.GenerateCube(cube.Position.ToUnity(), cube.Size.ToUnity(), (Substance)cube.Substance);
			var networkSync = primCube.AddComponent<NetworkSync>();
			networkSync.Id = cube.Id;
		}
		public static void UpdateCube(NetworkCube cube)
		{
			if (NetworkSync.NetworkSyncs.TryGetValue(cube.Id, out var sync))
			{
				sync.UpdateSync(cube);
			}
			else
			{
				CreateCube(cube);
			}
		}

		public static void UpdateChunk(NetworkChunk chunk)
		{
			foreach (var cube in chunk.Cubes)
			{
				UpdateCube(cube);
			}

		}


		public static void GenerateNewChunk(UnityEngine.Vector2Int position)
		{
			GenerateNextChunk = true;
			CubeGenerator.GenerateNewChunk(position);

			GenerateNextChunk = false;
		}
	}
}
