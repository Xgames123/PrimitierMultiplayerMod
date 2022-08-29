using PrimitierMultiplayer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimitierMultiplayer.Shared.Packets.c2s2c
{
	public class CubeChunkChangePacket
	{
		public NetworkCube Cube { get; set; }
		public Vector2 OldChunk { get; set; }
		public Vector2 NewChunk { get; set; }
	}
}
