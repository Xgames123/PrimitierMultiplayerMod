using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierServer.Shared.Packets.s2c
{
	public class ServerUpdatePacket
	{
		public NetworkPlayer[] Players { get; set; }

		public NetworkChunk[] Chunks { get; set; }
	}
}
