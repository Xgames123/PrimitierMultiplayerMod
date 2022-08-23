﻿using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared.Models
{
	public enum NetworkChunkType
	{
		Normal,
		Empty,
		Broken,

	}

	public struct NetworkChunk : INetworkModel
	{
		public static NetworkChunk NewEmptyChunk() { return new NetworkChunk() { ChunkType = NetworkChunkType.Empty }; }
		public static NetworkChunk NewBrokenChunk() { return new NetworkChunk() { ChunkType = NetworkChunkType.Broken }; }

		public NetworkChunkType ChunkType;
		
		public int Owner;
		public List<NetworkCube> Cubes;


		public void Serialize(NetDataWriter writer)
		{
			writer.Put((byte)ChunkType);
			if (ChunkType == NetworkChunkType.Broken || ChunkType == NetworkChunkType.Empty)
				return;

			writer.PutList(Cubes);
			writer.Put(Owner);
		}

		public void Deserialize(NetDataReader reader)
		{
			ChunkType = (NetworkChunkType)reader.GetByte();
			if (ChunkType != NetworkChunkType.Normal)
				return;

			Cubes = reader.GetList<NetworkCube>();
			Owner = reader.GetInt();
		}


	}


}
