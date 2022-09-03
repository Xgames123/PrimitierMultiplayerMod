using PrimitierMultiplayer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimitierMultiplayer.Shared.Packets.s2c
{
	public class CubeChunkChangePacket : Packet
	{
		public NetworkCube Cube { get; set; }
		public Vector2 OldChunk { get; set; }
	}
}
