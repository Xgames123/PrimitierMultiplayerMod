using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared.Models
{
	public enum NetworkChunkType
	{
		Normal,
		Broken,

	}

	public struct NetworkChunk : INetworkModel
	{
		public static NetworkChunk NewEmptyChunk() { return new NetworkChunk() { ChunkType = NetworkChunkType.Normal, Cubes = new List<NetworkCube>(), Owner=-1 }; }
		public static NetworkChunk NewBrokenChunk() { return new NetworkChunk() { ChunkType = NetworkChunkType.Broken, Owner =-1 }; }

		public NetworkChunkType ChunkType;
		
		public int Owner;
		public List<NetworkCube> Cubes;


		public void Serialize(NetDataWriter writer)
		{
			writer.Put((byte)ChunkType);
			if (ChunkType == NetworkChunkType.Broken)
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
