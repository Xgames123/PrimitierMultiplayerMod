using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib.Utils;

namespace PrimitierServer.Shared.Config
{
	public struct NetworkDebugConfig : INetSerializable
	{
		public bool ShowLocalPlayer;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(ShowLocalPlayer);
		}

		public void Deserialize(NetDataReader reader)
		{
			ShowLocalPlayer = reader.GetBool();
		}

		
	}
}
