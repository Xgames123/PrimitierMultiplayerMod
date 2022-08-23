using LiteNetLib;
using PrimitierMultiplayer.Mod.Components;
using PrimitierMultiplayer.Shared.PacketHandling;
using PrimitierMultiplayer.Shared.Packets.s2c;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Mod.PacketHandlers
{
	public class PlayerJoinedPacketHandler : PacketHandler<PlayerJoinedPacket>
	{
		public override void HandelPacket(PlayerJoinedPacket packet, NetPeer peer)
		{
			RemotePlayer.Create(packet.initialPlayerData);

		}
	}
}
