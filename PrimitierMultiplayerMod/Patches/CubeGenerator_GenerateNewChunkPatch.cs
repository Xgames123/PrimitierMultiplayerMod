using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod.Patches
{
	[HarmonyPatch(typeof(CubeGenerator), nameof(CubeGenerator.GenerateNewChunk))]
	public class CubeGenerator_GenerateNewChunkPatch
	{
		private static bool Prefix(UnityEngine.Vector2Int chunkPos)
		{
			if (MultiplayerManager.IsInMultiplayerMode)
			{
				return true;
				if (ChunkManager.AllowGenerateNextChunk)
					return true;

				return false;
			}

			return true;

		}

	}
}
