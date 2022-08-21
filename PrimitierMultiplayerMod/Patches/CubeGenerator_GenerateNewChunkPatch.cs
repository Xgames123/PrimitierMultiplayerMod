﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Mod.Patches
{
	[HarmonyPatch(typeof(CubeGenerator), nameof(CubeGenerator.GenerateNewChunk))]
	public class CubeGenerator_GenerateNewChunkPatch
	{
		private static bool Prefix(UnityEngine.Vector2Int chunkPos)
		{
			if (MultiplayerManager.IsInMultiplayerMode)
			{
				if (WorldManager.AllowGenerateNextChunk)
					return true;

				return false;
			}

			return true;

		}

	}
}
