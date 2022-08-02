using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimitierMultiplayerMod.Patches
{
	[HarmonyPatch(typeof(CubeGenerator), nameof(CubeGenerator.GenerateChunk))]
	public class CubeGenerator_GenerateChunk
	{
		private static void Postfix(Vector2Int chunkPos)
		{
			if (!MultiplayerManager.IsInMultiplayerMode)
			{
				return;
			}
			
			
		}

	}
}
