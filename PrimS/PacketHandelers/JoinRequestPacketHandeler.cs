using LiteNetLib;
using PrimitierMultiplayer.Shared.PacketHandeling;
using PrimitierMultiplayer.Shared.Packets.c2s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Server.PacketHandelers
{
	public class JoinRequestPacketHandler : PacketHandler<JoinRequestPacket>
	{
		public override void HandelPacket(JoinRequestPacket packet, NetPeer peer)
		{
			
			
		}

	}
}
