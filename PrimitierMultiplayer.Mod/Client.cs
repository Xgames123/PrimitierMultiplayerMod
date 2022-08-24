using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PrimitierMultiplayer.Shared.Packets.s2c;
using PrimitierMultiplayer.Shared.Packets.c2s;
using PrimitierMultiplayer.Shared;
using UnityEngine;
using System.Reflection;
using PrimitierModdingFramework;
using PrimitierMultiplayer.Mod.Components;
using PrimitierMultiplayer.Shared.Packets;
using PrimitierMultiplayer.Shared.Models;
using PrimitierMultiplayer.Shared.PacketHandling;

namespace PrimitierMultiplayer.Mod
{
	public class Client
	{
		public EventBasedNetListener Listener;
		public NetManager NetManager;

		public bool IsConnected { get { return NetManager.ConnectedPeersCount >= 1; } }


		public NetPeer Server { get; private set; } = null;
		public PacketHandlerContainer PacketHandlerContainer;

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
			_packetProcessor = new NetPacketProcessor();

			Listener.NetworkReceiveEvent += NetworkReceiveEvent;
			Listener.NetworkErrorEvent += NetworkErrorEvent;
			Listener.PeerDisconnectedEvent += DisconnectedEvent;
			Listener.PeerConnectedEvent += ConnectedEvent;

			
		}

		public void Connect(string address, int port)
		{
			if (IsConnected)
				Disconnect();


			PacketHandlerContainer = new PacketHandlerContainer(ref NetManager, ref _packetProcessor, ref _writer);
			PacketProcessorTypeRegister.RegisterNetworkModels(ref _packetProcessor);

			PacketHandlerContainer.AddPacketHandlers(Assembly.GetExecutingAssembly());


			Mod.Chat.AddSystemMessage("Connecting to the server");

			PMFLog.Message("Starting client");
			NetManager.Start();

			PMFLog.Message($"Connecting to {address}:{port}...");

			_writer.Reset();
			_writer.Put(Mod.ModVersion.ToString());
			NetManager.Connect(address, port, _writer);
		}

		public void Disconnect()
		{
			NetManager.DisconnectAll();
			PMFLog.Message("Disconnecting from server");
		}

		public void Update()
		{
			if (NetManager == null)
				return;

			NetManager.PollEvents();
		}

		public void Stop()
		{
			NetManager.Stop();

		}

		public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod) where T : class, new()
		{
			_writer.Reset();
			_packetProcessor.Write<T>(_writer, packet);
			Server.Send(_writer, deliveryMethod);
		}


		private void ConnectedEvent(NetPeer peer)
		{
			Server = peer;
			PMFLog.Message("Connected to the server");

			SendPacket(new JoinRequestPacket() { Username = PlayerInfo.Username, StaticPlayerId = PlayerInfo.StaticId }, DeliveryMethod.ReliableOrdered);
		}

		private void DisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
		{
			MultiplayerManager.Stop();
			Server = null;
			ErrorGenerator.ReadError(ref disconnectInfo.AdditionalData, ref _packetProcessor);

			Mod.Chat.AddSystemMessage($"Disconnected from the server Reason:{disconnectInfo.Reason}");
			
		}

		private void NetworkErrorEvent(IPEndPoint endPoint, System.Net.Sockets.SocketError socketError)
		{
			PMFLog.Error("Got network error: "+socketError);
		}

		private void NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
		{
			PacketHandlerContainer.ReadAllPackets(reader, peer);
		}



	}
}
