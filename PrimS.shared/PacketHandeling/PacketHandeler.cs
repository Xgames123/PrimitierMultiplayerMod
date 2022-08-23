using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared.PacketHandeling
{
	public abstract class PacketHandeler
	{

		public abstract void RegisterInPacketProcessor(ref NetPacketProcessor processor);


	}

	public abstract class PacketHandler<T> : PacketHandeler where T : class,new()
	{
		public override void RegisterInPacketProcessor(ref NetPacketProcessor processor)
		{
			processor.SubscribeReusable<T, NetPeer>(HandelPacket);
		}


		public abstract void HandelPacket(T packet, NetPeer peer);

	}
}
