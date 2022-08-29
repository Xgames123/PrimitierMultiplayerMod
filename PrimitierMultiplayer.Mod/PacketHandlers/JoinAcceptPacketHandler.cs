using LiteNetLib;
using PrimitierModdingFramework;
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
	public class JoinAcceptPacketHandler : PacketHandler<JoinAcceptPacket>
	{
		public override void HandelPacket(JoinAcceptPacket packet, NetPeer peer)
		{
			MultiplayerManager.LocalId = packet.Id;
	

			foreach (var playerInGame in packet.PlayersAlreadyInGame)
			{
				RemotePlayer.Create(playerInGame);
			}



			ConfigManager.ClientConfig = packet.ClientConfig;
			ConfigManager.Debug = packet.Debug;
			ConfigManager.DebugConfig = packet.DebugConfig;

			MultiplayerManager.EnterGame(packet.WorldSeed, packet.Position.ToUnity());
			if (packet.DebugConfig.ShowChunkBounds)
			{
				if(!ChunkBoundViewer.IsCreated)
					ChunkBoundViewer.Create();
			}
		}
	}
}
