using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using LiteNetLib.Utils;

namespace PrimitierMultiplayer.Shared.Models
{
	public struct NetworkChunkPositionPair : INetworkModel
	{
		public NetworkChunk Chunk;
		public Vector2 Position;

		public NetworkChunkPositionPair(KeyValuePair<Vector2, NetworkChunk> keyValuePair)
		{
			Chunk = keyValuePair.Value;
			Position = keyValuePair.Key;
		}

		public NetworkChunkPositionPair(NetworkChunk chunk, Vector2 position)
		{
			Chunk = chunk;
			Position = position;
		}

		public void Deserialize(NetDataReader reader)
		{
			Chunk = reader.Get<NetworkChunk>();
			Position = reader.GetVector2();
		}

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(Chunk);
			writer.Put(Position);
		}
	}
}
