using LiteNetLib;
using LiteNetLib.Utils;

using PrimitierMultiplayer.Shared.PacketHandling;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared
{
	public abstract class Peer
	{
		public EventBasedNetListener Listener;
		public NetManager NetManager;
		public PacketHandlerContainer PacketHandlerContainer;

		private NetPacketProcessor _packetProcessor;
		protected NetDataWriter Writer;

		public bool IsRunning { get { return NetManager.IsRunning; } }


		public Peer()
		{
			Listener = new EventBasedNetListener();
			NetManager = new NetManager(Listener)
			{
				IPv6Enabled = IPv6Mode.Disabled,
				AutoRecycle = true
			};
		
			Listener.ConnectionRequestEvent += ConnectionRequestEvent;
			Listener.PeerConnectedEvent += PeerConnectedEvent;
			Listener.PeerDisconnectedEvent += PeerDisconnectedEvent;
			Listener.NetworkErrorEvent += NetworkErrorEvent;
			Listener.NetworkReceiveEvent += NetworkReceiveEvent;
			
			Writer = new NetDataWriter();
			_packetProcessor = new NetPacketProcessor();

			PacketHandlerContainer = new PacketHandlerContainer(ref NetManager, ref _packetProcessor, ref Writer);
			PacketProcessorTypeRegister.RegisterNetworkModels(ref _packetProcessor);
		}

		

		protected virtual void NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
		{
			PacketHandlerContainer.ReadAllPackets(reader, peer);
		}

		protected abstract void NetworkErrorEvent(System.Net.IPEndPoint endPoint, System.Net.Sockets.SocketError socketError);

		protected void RejectConnectionRequest(ConnectionRequest request, ErrorCode errorCode)
		{
			Writer.Reset();
			ErrorGenerator.Generate(ref Writer, ref _packetProcessor, errorCode);
			request.Reject(Writer);
		}
		protected void ReadErrorFromDisconnectInfo(DisconnectInfo disconnectInfo)
		{
			ErrorGenerator.ReadError(ref disconnectInfo.AdditionalData, ref _packetProcessor);
		}

		public virtual void SendPacket<T>(NetPeer peer, T packet, DeliveryMethod deliveryMethod) where T : class, new()
		{
			Writer.Reset();
			_packetProcessor.Write<T>(Writer, packet);
			peer.Send(Writer, deliveryMethod);
		}
		public virtual void SendPacketToAll<T>(T packet, DeliveryMethod deliveryMethod) where T : class, new()
		{
			Writer.Reset();
			_packetProcessor.Write<T>(Writer, packet);
			NetManager.SendToAll(Writer, deliveryMethod);
		}

		public virtual void Update()
		{
			if (!IsRunning)
				return;

			NetManager.PollEvents();
		}
		public virtual void Stop()
		{
			NetManager.Stop(true);
		}

		protected virtual void PeerConnectedEvent(NetPeer peer) { }
		protected virtual void PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo) { }
		protected virtual void ConnectionRequestEvent(ConnectionRequest request) { }

		
	}
}
