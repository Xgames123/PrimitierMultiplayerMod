using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib.Utils;
using PrimitierMultiplayer.Shared;

namespace PrimitierMultiplayer.Shared.Models.Config
{
	public struct NetworkDebugConfig : INetworkModel
	{
		public bool ShowLocalPlayer;
		public bool ShowChunkBounds;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(ShowLocalPlayer);
			writer.Put(ShowChunkBounds);
		}

		public void Deserialize(NetDataReader reader)
		{
			ShowLocalPlayer = reader.GetBool();
			ShowChunkBounds = reader.GetBool();
		}

		
	}
}
