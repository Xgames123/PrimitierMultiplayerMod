using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared.PacketHandling
{
	public abstract class PacketHandler
	{
		protected NetDataWriter Writer;
		protected NetPacketProcessor PacketProcessor;
		protected NetManager NetManager;

		protected void SendPacketToAll<T>(T packet, DeliveryMethod deliveryMethod) where T : class, new()
		{
			Writer.Reset();
			PacketProcessor.Write<T>(Writer, packet);
			NetManager.SendToAll(Writer, deliveryMethod);
		}


		protected void SendPacket<T>(NetPeer peer, T packet, DeliveryMethod deliveryMethod) where T : class, new()
		{
			Writer.Reset();
			PacketProcessor.Write<T>(Writer, packet);
			peer.Send(Writer, deliveryMethod);
		}

		protected void SendPacket<T>(int peerId, T packet, DeliveryMethod deliveryMethod) where T : class, new()
		{
			
			SendPacket(NetManager.GetPeerById(peerId), packet, deliveryMethod);
		}


		public virtual void Setup(ref NetDataWriter writer, ref NetPacketProcessor packetProcessor, ref NetManager netManager)
		{
			Writer = writer;
			PacketProcessor = packetProcessor;
			NetManager = netManager;

		}


	}

	public abstract class PacketHandler<T> : PacketHandler where T : class,new()
	{

		public override void Setup(ref NetDataWriter writer, ref NetPacketProcessor packetProcessor, ref NetManager netManager)
		{
			base.Setup(ref writer, ref packetProcessor, ref netManager);
			packetProcessor.SubscribeReusable<T, NetPeer>(HandelPacket);
		}


		public abstract void HandelPacket(T packet, NetPeer peer);

	}
}
