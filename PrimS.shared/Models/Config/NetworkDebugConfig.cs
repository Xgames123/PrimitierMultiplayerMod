using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib.Utils;
using PrimitierServer.Shared;

namespace PrimitierMultiplayer.Shared.Models.Config
{
	public struct NetworkDebugConfig : INetworkModel
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
