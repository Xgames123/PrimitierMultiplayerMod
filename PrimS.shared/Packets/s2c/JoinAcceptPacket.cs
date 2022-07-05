using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimS.shared.Packets.s2c
{
	public class JoinAcceptPacket 
	{
		public int Id;

		public string Username { get; set; }
		public Vector3 Position { get; set; }

		public int WorldSeed { get; set; }
	}
}
