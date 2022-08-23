using LiteNetLib;
using PrimitierMultiplayer.Shared.PacketHandling;
using PrimitierMultiplayer.Shared.Packets.s2c;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Mod.PacketHandlers
{
	public class PlayerLeavePacketHandler : PacketHandler<PlayerLeavePacket>
	{
		public override void HandelPacket(PlayerLeavePacket packet, NetPeer peer)
		{
			var player = RemotePlayer.RemotePlayers[packet.Id];
			if (player != null)
			{
				Mod.Chat.AddServerMessage($"{player.FirstPersonNameTag.text} has left the game");
				RemotePlayer.DeletePlayer(player);
			}
		}
	}
}
