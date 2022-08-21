using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimitierMultiplayer.Shared
{
	public struct InitialPlayerData : INetSerializable
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public Vector3 Position { get; set; }

		public void Deserialize(NetDataReader reader)
		{
			Id = reader.GetInt();
			Username = reader.GetString();
			Position = reader.GetVector3();
		}

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(Id);
			writer.Put(Username);
			writer.Put(Position);
		}
	}
}
