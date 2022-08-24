﻿using PrimitierMultiplayer.Server.WorldStorage;
using PrimitierMultiplayer.Shared;
using PrimitierMultiplayer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Server.Mappers
{
	public static class ChunkMapperExtentions
	{
		public static NetworkChunk ToNetworkChunk(this StoredChunk storedChunk, int owner)
		{
			return new NetworkChunk() { ChunkType = NetworkChunkType.Normal, Owner = owner, Cubes = storedChunk.Cubes.ConvertAll((storedCube) => storedCube.ToNetworkCube()) };
		}

	}
}