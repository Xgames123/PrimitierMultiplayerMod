using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod.Patches
{
	[HarmonyPatch(typeof(TerrainGenerator), nameof(TerrainGenerator.Generate))]
	public class TerrainGenerator_GeneratePatch
	{
		private static void Postfix()
		{
			PrimitierModdingFramework.PMFLog.Message("TerrainGenerator_GeneratePatch");			
		}

	}
}
