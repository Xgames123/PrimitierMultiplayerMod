﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Mod.Patches
{
	[HarmonyPatch(typeof(TerrainGenerator), nameof(TerrainGenerator.Generate))]
	public class TerrainGenerator_GeneratePatch
	{
		private static void Prefix(UnityEngine.Vector2Int areaPos)
		{
			TerrainGenerator.worldSeed = WorldManager.WorldSeed;

		}

	}
}
