﻿using LiteNetLib;
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

		public bool IsConnected { get { return NetManager.ConnectedPeersCount >= 1; } }

		public bool IsInGame { get; private set; }

		public NetPeer Server { get; private set; } = null;

		public int LocalId { get; private set; } = -1;

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
			if (IsConnected)
				Disconnect();

			_packetProcessor = new NetPacketProcessor();
			_packetProcessor.RegisterNestedType<NetworkPlayer>();
			_packetProcessor.RegisterNestedType((writer, value) => writer.Put(value), reader => reader.GetVector3());
			_packetProcessor.RegisterNestedType((writer, value) => writer.PutList(value), reader => reader.GetList<InitialPlayerData>());

			_packetProcessor.SubscribeReusable<JoinAcceptPacket>(OnJoinAcceptPacket);
			_packetProcessor.SubscribeReusable<PlayerJoinedPacket>(OnPlayerJoinedPacket);
			_packetProcessor.SubscribeReusable<PlayerLeavePacket>(OnPlayerLeavePacket);
			_packetProcessor.SubscribeReusable<ServerUpdatePacket>(OnServerUpdatePacket);

			Mod.Chat.AddSystemMessage("Connecting to the server");

			PMFLog.Message("Starting client");
			NetManager.Start();

			PMFLog.Message($"Connecting to {address}:{port}...");
			NetManager.Connect(address, port, "");
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
			IsInGame = false;
			Server = null;
			PMFLog.Message("Disconnected from the server Reason:" + disconnectInfo.Reason);
			Mod.Chat.AddSystemMessage("Disconnected from the server");
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
			LocalId = packet.Id;

			foreach (var playerInGame in packet.PlayersAlreadyInGame)
			{
				OnPlayerJoinedPacket(playerInGame);
			} 

			IsInGame = true;
			TerrainGenerator.worldSeed = packet.WorldSeed;

			MultiplayerManager.EnterGame();
		}

		private void OnPlayerLeavePacket(PlayerLeavePacket packet)
		{
			var player = RemotePlayer.RemotePlayers[packet.Id];
			if (player != null)
			{
				Mod.Chat.AddServerMessage($"{player.NameTag.text} has left the game");
				RemotePlayer.DeletePlayer(player);
			}

		}


		private void OnPlayerJoinedPacket(PlayerJoinedPacket packet)
		{
			Mod.Chat.AddServerMessage($"{packet.Username} has Joined the game");

			RemotePlayer.Create(packet.Id, packet.Username, packet.Position.ToUnity());
		}

		private void OnServerUpdatePacket(ServerUpdatePacket packet)
		{
			
			foreach (var networkPlayer in packet.Players)
			{
				var remotePlayer = RemotePlayer.RemotePlayers[networkPlayer.Id];
				//PMFLog.Message($"NET PLAYER Position={networkPlayer.Position}; Position={networkPlayer.HeadPosition};");

				remotePlayer.transform.position = networkPlayer.Position.ToUnity();
				remotePlayer.Head.transform.position = networkPlayer.HeadPosition.ToUnity();
				remotePlayer.LHand.transform.position = networkPlayer.LHandPosition.ToUnity();
				remotePlayer.RHand.transform.position = networkPlayer.RHandPosition.ToUnity();

			}

		}

	}
}
