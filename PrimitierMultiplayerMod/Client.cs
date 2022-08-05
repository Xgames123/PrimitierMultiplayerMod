using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PrimitierServer.Shared.Packets.s2c;
using PrimitierServer.Shared.Packets.c2s;
using PrimitierServer.Shared;
using UnityEngine;
using System.Reflection;
using PrimitierModdingFramework;
using PrimitierMultiplayerMod.Components;
using MelonLoader.TinyJSON;

namespace PrimitierMultiplayerMod
{
	public class Client
	{
		public EventBasedNetListener Listener;
		public NetManager NetManager;

		public bool IsConnected { get { return NetManager.ConnectedPeersCount >= 1; } }


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
			_packetProcessor.RegisterNestedType<NetworkChunk>();
			_packetProcessor.RegisterNestedType<NetworkCube>();
			_packetProcessor.RegisterNestedType<NetworkPlayer>();
			_packetProcessor.RegisterNestedType<InitialPlayerData>();
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
			MultiplayerManager.Stop();
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

		private void CreateInitialPlayer(InitialPlayerData initialPlayerData)
		{
			Mod.Chat.AddServerMessage($"{initialPlayerData.Username} has Joined the game");

			RemotePlayer.Create(initialPlayerData.Id, initialPlayerData.Username, initialPlayerData.Position.ToUnity());
		}

		private void OnJoinAcceptPacket(JoinAcceptPacket packet)
		{
			LocalId = packet.Id;

			foreach (var playerInGame in packet.PlayersAlreadyInGame)
			{
				CreateInitialPlayer(playerInGame);
			} 

			MultiplayerManager.EnterGame(packet.WorldSeed);
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
			CreateInitialPlayer(packet.initialPlayerData);
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

			PMFLog.Message("Got server update");
			var testData = new PrimitierServer.Shared.NetworkChunk() { Cubes = new System.Collections.Generic.List<PrimitierServer.Shared.NetworkCube>() { new PrimitierServer.Shared.NetworkCube() { Id = 1, Position = new System.Numerics.Vector3(0, 0, 0), Size = new System.Numerics.Vector3(5, 5, 5), Substance = 0, Rotation = new System.Numerics.Quaternion(0, 0, 0, 0) } } };
			PMFLog.Message(JSON.Dump(testData));
			PMFLog.Message(packet.Chunks.Length);
			PMFLog.Message(JSON.Dump(packet.Chunks[0]));
			

			//ChunkManager.UpdateModChunk();
			//MainThreadRunner.EnqueueTask(()=>ChunkManager.UpdateModChunks(packet.Chunks));


		}

	}
}
