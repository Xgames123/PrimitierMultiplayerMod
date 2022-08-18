using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod.Patches
{
	[HarmonyPatch(typeof(CubeGenerator), nameof(CubeGenerator.DestroyChunks))]
	public class CubeGenerator_DestroyChunksPatch
	{
		private static bool Prefix(Il2CppSystem.Collections.Generic.List<UnityEngine.Vector2Int> destroyChunkPositions)
		{
			if (MultiplayerManager.IsInMultiplayerMode)
			{
				if (WorldManager.AllowDestroyNextChunk)
					return true;

				return false;
			}

			return true;

		}

	}
}
