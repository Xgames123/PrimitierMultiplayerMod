using PrimitierMultiplayer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimitierMultiplayer.Shared.Packets.s2c
{
	public class PlayerJoinedPacket : Packet
	{
		public InitialPlayerData initialPlayerData { get; set; }

	}
}
