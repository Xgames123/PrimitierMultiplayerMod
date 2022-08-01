using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimS.shared
{
	public class NetworkChunk
	{
		public bool Empty = false;

		public List<NetworkCube> Cubes = new List<NetworkCube>();

		public static NetworkChunk Deserialize(NetDataReader reader)
		{
			var empty = reader.GetBool();
			if (empty)
			{
				return new NetworkChunk() { Empty = true };
			}

			return new NetworkChunk() { Empty = false};
		}

		public static void Serialize(NetDataWriter writer, NetworkChunk chunk)
		{
			writer.Put(chunk.Empty);
			if (chunk.Empty)
				return;

		}
	}
}
