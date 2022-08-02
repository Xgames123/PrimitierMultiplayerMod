using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimS.shared
{
	public enum NetworkChunkType
	{
		Normal,
		Empty,
		Broken,

	}

	public class NetworkChunk
	{
		public static NetworkChunk EmptyChunk { get; private set; } = new NetworkChunk() { ChunkType = NetworkChunkType.Empty };
		public static NetworkChunk BrokenChunk { get; private set; } = new NetworkChunk() { ChunkType = NetworkChunkType.Broken };

		public NetworkChunkType ChunkType = NetworkChunkType.Normal;

		public List<NetworkCube> Cubes = new List<NetworkCube>();

		public static NetworkChunk Deserialize(NetDataReader reader)
		{
			var type = (NetworkChunkType)reader.GetByte();
			if (type == NetworkChunkType.Empty)
			{
				return EmptyChunk;
			}
			if(type == NetworkChunkType.Broken)
			{
				return BrokenChunk;
			}
			//TODO deserialize cubes

			return new NetworkChunk() { };
		}

		public static void Serialize(NetDataWriter writer, NetworkChunk chunk)
		{
			writer.Put((byte)chunk.ChunkType);
			if (chunk.ChunkType == NetworkChunkType.Broken || chunk.ChunkType == NetworkChunkType.Empty)
				return;

			//TODO serialize cubes

		}
	}
}
