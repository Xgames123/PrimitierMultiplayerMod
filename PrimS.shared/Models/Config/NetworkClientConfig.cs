using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib.Utils;
using PrimitierMultiplayer.Shared;
using PrimitierServer.Shared;

namespace PrimitierMultiplayer.Shared.Models.Config
{
	public struct NetworkClientConfig : INetworkModel
	{
		public int IdleUpdateDelay;
		public int ActiveUpdateDelay;


		public void Serialize(NetDataWriter writer)
		{
			writer.Put(IdleUpdateDelay);
			writer.Put(ActiveUpdateDelay);
		}

		public void Deserialize(NetDataReader reader)
		{
			IdleUpdateDelay = reader.GetInt();
			ActiveUpdateDelay = reader.GetInt();

		}

		
	}
}
