using PrimitierMultiplayer.Mod.Components;
using PrimitierMultiplayer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Mod.Mappers
{
	public static class ChunkMapperExtentions
	{

		public static NetworkChunk ToNetworkChunk(this RuntimeChunk runtimeChunk)
		{
			if (runtimeChunk == null)
				return NetworkChunk.NewBrokenChunk();

			if (runtimeChunk.NetworkSyncs.Count == 0)
				return NetworkChunk.NewEmptyChunk();

			var CubeList = new List<NetworkCube>(runtimeChunk.NetworkSyncs.Count);
			foreach (var syncId in runtimeChunk.NetworkSyncs)
			{
				var sync = CubeSync.GetById(syncId);
				CubeList.Add(sync.ToNetworkCube());
				
			}


			return new NetworkChunk() { Owner = runtimeChunk.Owner, ChunkType = NetworkChunkType.Normal, Cubes = CubeList };
		}
	}
}
