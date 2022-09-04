using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared.Models.Extentions
{
	public static class NetworkChunkPositionPairArrayExtentions
	{

		public static bool Contains(this NetworkChunkPositionPair[] array, System.Numerics.Vector2 position)
		{
			foreach (var item in array)
			{
				if (item.Position == position)
					return true;

			}
			return false;
		}
	}
}
