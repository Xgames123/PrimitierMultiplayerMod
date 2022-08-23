using PrimitierMultiplayer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared.Packets.s2c
{
	public class ServerUpdatePacket
	{
		public NetworkPlayer[] Players { get; set; }

		public NetworkChunkPositionPair[] Chunks { get; set; }
	}
}
