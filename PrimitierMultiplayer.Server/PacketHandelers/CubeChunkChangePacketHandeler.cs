using LiteNetLib;
using log4net;
using PrimitierMultiplayer.Server.WorldStorage;
using PrimitierMultiplayer.Shared.PacketHandling;
using PrimitierMultiplayer.Shared.Packets.c2s2c;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Server.PacketHandelers
{
	public class CubeChunkChangePacketHandeler : PacketHandler<CubeChunkChangePacket>
	{
		private static ILog c_log = LogManager.GetLogger(nameof(CubeChunkChangePacketHandeler));

		public override void HandelPacket(CubeChunkChangePacket packet, NetPeer peer)
		{
			var oldChunk = World.GetChunk(packet.OldChunk);
			if (oldChunk.Owner != peer.Id)
			{
#if DEBUG
				c_log.Debug("Client tries to send CubeChunkChangePacket on a chunk that is not owned by that client");
#endif
				return;
			}

			World.WriteCube(packet.NewChunk, packet.Cube);

			var newChunk = World.GetChunk(packet.NewChunk, false);
			if(newChunk.ChunkType != Shared.Models.NetworkChunkType.Broken && newChunk.Owner != -1)
			{
				SendPacket(newChunk.Owner, new CubeChunkChangePacket()
				{
					NewChunk = packet.NewChunk,
					OldChunk = packet.OldChunk,
					Cube = packet.Cube,
				}, DeliveryMethod.ReliableUnordered);
				

			}

		}
	}
}
