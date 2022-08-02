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
		private static bool Prefix(Vector2Int chunkPos)
		{

		}

	}
}
