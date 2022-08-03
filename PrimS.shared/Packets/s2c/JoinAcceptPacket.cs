using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimitierServer.shared.Packets.s2c
{
	public class JoinAcceptPacket 
	{
		public int Id { get; set; }

		public string Username { get; set; }
		public Vector3 Position { get; set; }

		public int WorldSeed { get; set; }

		public List<InitialPlayerData> PlayersAlreadyInGame { get; set; }
	}
}
