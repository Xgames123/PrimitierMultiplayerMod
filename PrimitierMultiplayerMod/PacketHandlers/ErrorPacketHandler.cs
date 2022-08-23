using LiteNetLib;
using PrimitierMultiplayer.Shared;
using PrimitierMultiplayer.Shared.PacketHandling;
using PrimitierMultiplayer.Shared.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Mod.PacketHandlers
{
	public class ErrorPacketHandler : PacketHandler<ErrorPacket>
	{
		public override void HandelPacket(ErrorPacket packet, NetPeer peer)
		{
			var message = packet.Message;
			if (message == null)
			{
				message = ErrorGenerator.ErrorCodeToString(packet.ErrorCode);
			}

			Mod.Chat.AddServerMessage($"ErrorCode: {packet.ErrorCode} {message}");
		}
	}
}
