using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimS.shared.Packets.s2c
{
	public class SuccessfullyConnectedPacket : Packet
	{
		public override PacketId PacketId => shared.PacketId.SuccessfullyConnected;

		public int Id;
		public string Username;
		public Vector3 Position;

		public SuccessfullyConnectedPacket(NetDataReader reader) : base(reader)
		{
			Id = reader.GetInt();
			Username = reader.GetString(10);
			Position = reader.GetVector3();

		}
		public SuccessfullyConnectedPacket(int id, string username, Vector3 position)
		{
			Username = username;
			Id = id;
			Position = position;
		}

		public override void PutOnWriter(ref NetDataWriter writer)
		{
			writer.Put(Id);
			writer.Put(Username);
			writer.PutVector3(Position);

		}
	}
}
