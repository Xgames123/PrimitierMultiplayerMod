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
			//TODO: remove this server should be trusted
			var chunk = WorldManager.GetVisibleChunk(ChunkMath.WorldToChunkPos(packet.Cube.Position));
			if (chunk.Owner != MultiplayerManager.LocalId)
			{
				PMFLog.Warning("Got CubeChunkChangePacket for a chunk not owned by this client (Ignoring packet)");
				return;
			}
				

			WorldManager.UpdateCube(packet.Cube);
		}
	}
}
