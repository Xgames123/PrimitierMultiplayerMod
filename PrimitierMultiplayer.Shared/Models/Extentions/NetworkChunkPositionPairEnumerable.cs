using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared.Models.Extentions
{
	public static class NetworkChunkPositionPairEnumerable
	{

		public static bool Contains(this IEnumerable<NetworkChunkPositionPair> container, System.Numerics.Vector2 position)
		{
			foreach (var item in container)
			{
				if (item.Position == position)
					return true;

			}
			return false;
		}
	}
}
