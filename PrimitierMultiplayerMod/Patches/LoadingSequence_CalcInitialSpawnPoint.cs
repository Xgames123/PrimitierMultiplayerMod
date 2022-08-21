using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Mod.Patches
{
	[HarmonyPatch(typeof(LoadingSequence), nameof(LoadingSequence.CalcInitialSpawnPoint))]
	public class LoadingSequence_CalcInitialSpawnPoint
	{

		public static bool Prefix(ref UnityEngine.Vector3 __result)
		{
			__result = WorldManager.PlayerStartPosition;
			return false;
		}
	}
}
