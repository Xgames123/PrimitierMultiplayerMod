using PrimitierMultiplayer.Shared.PacketHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using PrimitierMultiplayer.Shared.Packets.s2c;
using PrimitierMultiplayer.Mod.Components;
using PrimitierMultiplayer.Shared;
using PrimitierModdingFramework;

namespace PrimitierMultiplayer.Mod.PacketHandlers
{
	public class CubeChunkChangePacketHandler : PacketHandler<CubeChunkChangePacket>
	{
		public override void HandelPacket(CubeChunkChangePacket packet, NetPeer peer)
		{
			if (!WorldManager.IsOwned(ChunkMath.WorldToChunkPos(packet.Cube.Position)))
			{
				PMFLog.Warning("Got CubeChunkChangePacket for a chunk not owned by this client (Ignoring packet)");
				return;
			}
				

			WorldManager.UpdateCube(packet.Cube);
		}
	}
}
