#if false
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod.Patches
{
	[HarmonyPatch(typeof(CubeGenerator), nameof(CubeGenerator.GenerateCube))]
	public class CubeGenerator_GenerateNewChunkPatch
	{
		private static void Postfix()
		{
			PrimitierModdingFramework.PMFLog.Message("CubeGenerator_GenerateNewChunkPatch");

		}

	}
}
#endif