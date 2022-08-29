using PrimitierMultiplayer.Shared.PacketHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimitierMultiplayer.Shared.Packets.c2s2c;
using LiteNetLib;
using PrimitierMultiplayer.Mod.Components;

namespace PrimitierMultiplayer.Mod.PacketHandlers
{
	public class CubeChunkChangePacketHandler : PacketHandler<CubeChunkChangePacket>
	{
		public override void HandelPacket(CubeChunkChangePacket packet, NetPeer peer)
		{
			if (!WorldManager.IsOwned(packet.NewChunk))
				return;

			WorldManager.UpdateCube(packet.Cube, packet.NewChunk);
		}
	}
}
