﻿using LiteNetLib;
using log4net;
using PrimitierMultiplayer.Server.WorldStorage;
using PrimitierMultiplayer.Shared.PacketHandling;
using PrimitierMultiplayer.Shared.Packets.c2s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimitierMultiplayer.Shared.Packets.s2c;

namespace PrimitierMultiplayer.Server.PacketHandelers
{
	public class PlayerUpdatePacketHandler : PacketHandler<PlayerUpdatePacket>
	{
		private ILog _log = LogManager.GetLogger(nameof(PlayerUpdatePacketHandler));

		public override void HandelPacket(PlayerUpdatePacket packet, NetPeer peer)
		{
			var player = PlayerManager.GetPlayerById(peer.Id);
			if (player == null)
			{
				_log.Error("Got update form non joined player");
				return;
			}

			//_log.Debug($"PLAYER UPDATE Position={packet.Position}; HeadPosition={packet.HeadPosition}; RHandPosition={packet.RHandPosition}; LHandPosition={packet.LHandPosition};");

			player.Position = packet.Position;
			player.HeadPosition = packet.HeadPosition;
			player.RHandPosition = packet.RHandPosition;
			player.LHandPosition = packet.LHandPosition;

			foreach (var chunkPosPair in packet.Chunks)
			{
				var cachedChunk = World.GetChunk(chunkPosPair.Position);
				if (cachedChunk.Owner != peer.Id)
					continue;

				if (chunkPosPair.Chunk.ChunkType == Shared.Models.NetworkChunkType.Normal)
				{
					foreach (var cube in chunkPosPair.Chunk.Cubes)
					{
						if (cube.IsInWrongChunk)
						{
							World.WriteCube(cube);
							SendPacket(peer, new CubeChunkChangePacket()
							{
								Cube = cube,
								OldChunk = chunkPosPair.Position

							}, DeliveryMethod.ReliableOrdered);
						}

					}
				}


				World.WriteChunk(chunkPosPair);
			}

		}
	}
}
