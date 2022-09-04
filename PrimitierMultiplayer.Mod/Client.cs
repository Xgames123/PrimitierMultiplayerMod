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
using System.Diagnostics;


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

		private Stopwatch _updateStopwatch = Stopwatch.StartNew();

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

			if (MultiplayerManager.IsInMultiplayerMode && Server != null)
			{
				var updateDelay = ConfigManager.ClientConfig.ActiveUpdateDelay;
				//TODO: use idel update delay when client is idel
				if (_updateStopwatch.ElapsedMilliseconds >= updateDelay)
				{
					_updateStopwatch.Restart();
					SendUpdatePackets();
				}
			}

		}
		private void SendUpdatePackets()
		{

			NetworkChunkPositionPair[] chunks = new NetworkChunkPositionPair[WorldManager.OwnedChunks.Count];

			for (int i = 0; i < WorldManager.OwnedChunks.Count; i++)
			{
				var ownedChunkPos = WorldManager.OwnedChunks[i];
				var ownedChunk = WorldManager.GetChunk(ownedChunkPos);

				var netChunk = ownedChunk.UpdateToServer();

				chunks[i] = new NetworkChunkPositionPair(netChunk, ownedChunkPos);
			}

			PMFLog.Message($"sending {chunks.Length} chunks");
			foreach (var chunk in chunks)
			{
				PMFLog.Message($"Pos: X: {chunk.Position.X}, Y: {chunk.Position.Y}");
				PMFLog.Message($"Owner: {chunk.Chunk.Owner}");
				PMFLog.Message($"cubeCount: {chunk.Chunk.Cubes.Count}");

			}

			var packet = new PlayerUpdatePacket()
			{
				Position = PMFHelper.CameraRig.transform.position.ToNumerics(),
				HeadPosition = Camera.main.transform.position.ToNumerics(),
				LHandPosition = PMFHelper.LHand.transform.position.ToNumerics(),
				RHandPosition = PMFHelper.RHand.transform.position.ToNumerics(),

				Chunks = chunks,
			};


			SendPacket(packet, DeliveryMethod.Unreliable);

		}

		public void Stop()
		{
			NetManager.Stop();

		}

		public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod) where T : class, new()
		{
			if (Server == null)
				throw new Exception("you tried to send a packet while not being connected to the server");

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
