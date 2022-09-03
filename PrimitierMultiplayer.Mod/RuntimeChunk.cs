using PrimitierMultiplayer.Mod.Components;
using PrimitierMultiplayer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Mod
{
	public class RuntimeChunk
	{
		public int Owner;

		public List<uint> NetworkSyncs = new List<uint>();

		public NetworkChunk UpdateToServer()
		{

			if (NetworkSyncs.Count == 0)
				return NetworkChunk.NewEmptyChunk();

			var CubeList = new List<NetworkCube>(NetworkSyncs.Count);
			foreach (var syncId in NetworkSyncs.ToArray())
			{
				var sync = CubeSync.GetById(syncId);
				var netcube = sync.UpdateToServer();
				if (netcube.Id == default)
					continue;
				CubeList.Add(netcube);

			}


			return new NetworkChunk() { Owner = Owner, ChunkType = NetworkChunkType.Normal, Cubes = CubeList };
		}
	}
}
