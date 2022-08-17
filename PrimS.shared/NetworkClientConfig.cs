using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib.Utils;

namespace PrimitierServer.Shared
{
	public struct NetworkClientConfig : INetSerializable
	{
		public int IdleUpdateDelay;
		public int ActiveUpdateDelay;

		//TODO: notify use that the server is a debug server
		public bool Debug;
		public bool ShowLocalPlayer;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(IdleUpdateDelay);
			writer.Put(ActiveUpdateDelay);
			writer.Put(Debug);
			if (Debug)
			{
				writer.Put(ShowLocalPlayer);
			}
		}

		public void Deserialize(NetDataReader reader)
		{
			IdleUpdateDelay = reader.GetInt();
			ActiveUpdateDelay = reader.GetInt();
			Debug = reader.GetBool();
			if (Debug)
			{
				ShowLocalPlayer = reader.GetBool();
			}
			else
			{
				ShowLocalPlayer = false;

			}

		}

		
	}
}
