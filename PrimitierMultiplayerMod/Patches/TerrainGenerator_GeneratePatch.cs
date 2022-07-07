using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod.Patches
{
	[HarmonyPatch(typeof(CubeGenerator), nameof(CubeGenerator.GenerateChunk))]
	public class CubeGenerator_GenerateChunkPatch
	{
		private static void Postfix()
		{
			PrimitierModdingFramework.PMFLog.Message("CubeGenerator_GenerateChunkPatch");

		}

	}
}
