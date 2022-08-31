using LiteNetLib.Utils;
using PrimitierMultiplayer.Shared.Models;
using PrimitierMultiplayer.Shared.Models.Config;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimitierMultiplayer.Shared.Packets.s2c
{
	public class JoinAcceptPacket : Packet
	{
		public int Id { get; set; }

		public string Username { get; set; }
		public Vector3 Position { get; set; }

		public int WorldSeed { get; set; }

		public List<InitialPlayerData> PlayersAlreadyInGame { get; set; }

		public NetworkClientConfig ClientConfig { get; set; }

		//TODO: notify client when connecting to debug server
		public bool Debug { get; set; }
		public NetworkDebugConfig DebugConfig { get; set; }

		
	}
}
