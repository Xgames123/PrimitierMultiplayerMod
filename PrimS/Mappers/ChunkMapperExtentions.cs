using PrimitierServer.Shared;
using PrimitierServer.WorldStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierServer.Mappers
{
	public static class ChunkMapperExtentions
	{
		public static NetworkChunk ToNetworkChunk(this StoredChunk storedChunk, int owner)
		{
			return new NetworkChunk() { ChunkType = NetworkChunkType.Normal, Owner = owner, Cubes = storedChunk.Cubes.ConvertAll<NetworkCube>((storedCube)=> storedCube.ToNetworkCube()) };
		}

	}
}
