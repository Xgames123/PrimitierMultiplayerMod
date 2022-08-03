using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierServer.Shared.Packets.c2s
{
	public class JoinRequestPacket
	{
		public string Username { get; set; }
		public string StaticPlayerId { get; set; }

	}
}
