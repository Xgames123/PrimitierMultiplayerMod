using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimS.shared.Packets.c2s
{
	public class ConnectionRequestPacket : Packet
	{
		public override PacketId PacketId => PacketId.ConnectionRequest;

		public string Username;

		public string PrimitierVersion;
		public string MultiplayerModVersion;

		public ConnectionRequestPacket(string username, string primitierVersion, string multiplayerModVersion)
		{
			Username = username;
			PrimitierVersion = primitierVersion;
			MultiplayerModVersion = multiplayerModVersion;
		}
		public ConnectionRequestPacket(NetDataReader reader) : base(reader)
		{
			Username = reader.GetString(10);
			PrimitierVersion = reader.GetString(10);
			MultiplayerModVersion = reader.GetString(10);
		}
		

		public override void PutOnWriter(ref NetDataWriter writer)
		{
			writer.Put(Username);
			writer.Put(PrimitierVersion);
			writer.Put(MultiplayerModVersion);

			
		}
	}
}
