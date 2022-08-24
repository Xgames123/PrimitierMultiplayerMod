using PrimitierMultiplayer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimitierMultiplayer.Shared.Packets.c2s
{
	public class PlayerUpdatePacket
	{
		public Vector3 Position { get; set; }
		public Vector3 HeadPosition { get; set; }
		public Vector3 LHandPosition { get; set; }
		public Vector3 RHandPosition { get; set; }

		public NetworkChunkPositionPair[] Chunks { get; set; }
	}
}
