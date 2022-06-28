using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PrimS.shared.Packets.s2c;
using PrimS.shared.Packets.c2s;
using PrimS.shared;
using UnityEngine;
using System.Reflection;
using PrimitierModdingFramework;

namespace PrimitierMultiplayerMod
{
	public class Client
	{
		public EventBasedNetListener Listener;
		public NetManager NetManager;

		public NetPeer Server { get; private set; } = null;

		private NetPacketProcessor _packetProcessor;
		private NetDataWriter _writer;

		public Client()
		{
			_writer = new NetDataWriter();
			Listener = new EventBasedNetListener();
			NetManager = new NetManager(Listener)
			{
				AutoRecycle = true,
			};

			Listener.NetworkReceiveEvent += NetworkReceiveEvent;
			Listener.NetworkErrorEvent += NetworkErrorEvent;
			Listener.PeerDisconnectedEvent += DisconnectedEvent;
			Listener.PeerConnectedEvent += ConnectedEvent;

		}

		public void Connect(string address, int port)
		{
			_packetProcessor = new NetPacketProcessor();
			_packetProcessor.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetVector3());
			_packetProcessor.SubscribeReusable<JoinAcceptPacket>(OnJoinAcceptPacket);

			PMFLog.Message("Starting client");
			NetManager.Start();

			PMFLog.Message($"Connecting to {address}:{port}...");
			NetManager.Connect(address, port, "");
		}

		public void Update()
		{
			NetManager.PollEvents();
		}

		public void Stop()
		{
			NetManager.Stop();

		}

		private void SendPacket<T>(T packet, DeliveryMethod deliveryMethod) where T : class, new()
		{
			_writer.Reset();
			_packetProcessor.Write<T>(packet);
			Server.Send(_writer, deliveryMethod);
		}


		private void ConnectedEvent(NetPeer peer)
		{
			Server = peer;
			PMFLog.Message("Connected to the server");

			SendPacket(new JoinRequestPacket() { Username = PlayerInfo.Username }, DeliveryMethod.ReliableOrdered);
		}

		private void DisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
		{
			Server = null;
			PMFLog.Message("Disconnected from the server Reason:" + disconnectInfo.Reason);
		}

		private void NetworkErrorEvent(IPEndPoint endPoint, System.Net.Sockets.SocketError socketError)
		{
			PMFLog.Error("Got network error: "+socketError);
		}

		private void NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
		{
			_packetProcessor.ReadAllPackets(reader);
		}

		private void OnJoinAcceptPacket(JoinAcceptPacket packet)
		{
			PMFLog.Message("Successfully joined the game");
		}



	}
}
