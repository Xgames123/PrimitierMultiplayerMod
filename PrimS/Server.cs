using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;
using log4net;
using log4net.Core;
using PrimitierServer.Shared.Packets;
using PrimitierServer.Shared.Packets.c2s;
using PrimitierServer.Shared.Packets.s2c;
using PrimitierServer.Shared;
using System.Numerics;
using System.Diagnostics;
using PrimitierServer.Mappers;
using PrimitierServer.WorldStorage;
using PrimitierServer.Shared;

namespace PrimitierServer
{
	public class Server
	{
		public EventBasedNetListener Listener;
		public NetManager NetManager;

		public bool IsStarted { get; private set; } = false;

		private ILog _log = LogManager.GetLogger(nameof(Server));

		private NetPacketProcessor _packetProcessor;
		private NetDataWriter _writer;

		private Stopwatch UpdateStopwatch = Stopwatch.StartNew();

		public Server()
		{
			Listener = new EventBasedNetListener();
			NetManager = new NetManager(Listener)
			{
				IPv6Enabled = IPv6Mode.Disabled,
				AutoRecycle = true
			};

			Listener.ConnectionRequestEvent += ConnectionRequestEvent;
			Listener.PeerDisconnectedEvent += PeerDisconnectedEvent;
			Listener.NetworkErrorEvent += NetworkErrorEvent;
			Listener.NetworkReceiveEvent += NetworkReceiveEvent;

			_writer = new NetDataWriter();
			_packetProcessor = new NetPacketProcessor();
			PacketProcessorTypeRegister.RegisterNetworkTypes(ref _packetProcessor);

			_packetProcessor.SubscribeReusable<JoinRequestPacket, NetPeer>(OnJoinRequest);
			_packetProcessor.SubscribeReusable<PlayerUpdatePacket, NetPeer>(OnPlayerUpdate);

			ConfigLoader.OnConfigReload += OnConfigReload;
			OnConfigReload(ConfigLoader.Config);
		}

		private void NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
		{
			_packetProcessor.ReadAllPackets(reader, peer);
		}

		private void OnConfigReload(ConfigFile? config)
		{
			if (config == null)
			{
				_log.Error("Config was null. Ignoring...");
				return;
			}
			Stop();

			NetManager.Start(config.ListenIp, "", config.ListenPort);
			_log.Info($"Started server on {config.ListenIp}:{NetManager.LocalPort}");


		}

		private void SendPacketToAll<T>(T packet, DeliveryMethod deliveryMethod) where T : class, new()
		{
			_writer.Reset();
			_packetProcessor.Write<T>(_writer, packet);
			NetManager.SendToAll(_writer, deliveryMethod);
		}


		private void SendPacket<T>(NetPeer peer, T packet, DeliveryMethod deliveryMethod) where T : class, new()
		{
			
			_writer.Reset();
			_packetProcessor.Write<T>(_writer, packet);
			peer.Send(_writer, deliveryMethod);
		}


		private void NetworkErrorEvent(System.Net.IPEndPoint endPoint, System.Net.Sockets.SocketError socketError)
		{
			_log.Error("Got network error: " + socketError);
		}

		private void PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
		{
			var player = PlayerManager.GetPlayerById(peer.Id);

			if (player == null)
			{
				_log.Info($"{peer.EndPoint} left the game");
				return;
			}
			_log.Info($"{player.Username} left the game");

			PlayerManager.DeletePlayer(player.RuntimeId);
			SendPacketToAll(new PlayerLeavePacket() { Id = peer.Id }, DeliveryMethod.ReliableOrdered);
		}



		private void ConnectionRequestEvent(ConnectionRequest request)
		{
			_log.Info(request.RemoteEndPoint + " has requested a connection");


			if (NetManager.ConnectedPeersCount >= ConfigLoader.Config.MaxPlayers)
			{
				_writer.Reset();
				ErrorGenerator.Generate(ref _writer, ref _packetProcessor, Shared.ErrorCode.ServerFull);
				request.Reject(_writer);
				return;
			}

			var reader = request.Data;
			Version? modVersion = null;
			if (!Version.TryParse(reader.GetString(), out modVersion))
			{
				_writer.Reset();
				ErrorGenerator.Generate(ref _writer, ref _packetProcessor, Shared.ErrorCode.ProtocolError);
				request.Reject(_writer);
				return;
			}
			if (!SupportedVersions.CheckModVersion(modVersion))
			{
				_writer.Reset();
				ErrorGenerator.Generate(ref _writer, ref _packetProcessor, Shared.ErrorCode.UnsupportedModVersion);
				request.Reject(_writer);
			}


			request.Accept();

		}


		public void Update()
		{
			if (!NetManager.IsRunning)
				return;

			NetManager.PollEvents();

			if (ConfigLoader.Config == null)
				return;

			if(UpdateStopwatch.ElapsedMilliseconds >= ConfigLoader.Config.UpdateDelay)
			{
				UpdateStopwatch.Restart();

				SendUpdatePackets();
			}

		}
		private void SendUpdatePackets()
		{
			foreach (var peer in NetManager.ConnectedPeerList)
			{
				var currentPlayer = PlayerManager.GetPlayerById(peer.Id);

				if (currentPlayer == null)
					continue;

				var players = FindNetworkPlayersAroundPlayer(currentPlayer, 5);

				var chunks = FindNetworkChunksAroundPlayer(currentPlayer, 5);

				SendPacket<ServerUpdatePacket>(peer, new ServerUpdatePacket() { Players = players.ToArray(), Chunks = chunks.ToArray() }, DeliveryMethod.Unreliable);

			}

		}


		private List<NetworkChunkPositionPair> FindNetworkChunksAroundPlayer(RuntimePlayer currentPlayer, int radius)
		{
			var foundChunks = new List<NetworkChunkPositionPair>();


			var center = ChunkMath.WorldToChunkPos(currentPlayer.Position);

			var chunkRadius = ChunkMath.WorldToChunkRadius(radius);
			for (float x = center.X- chunkRadius; x < center.X + chunkRadius; x++)
			{
				for (float y = center.Y- chunkRadius; y < center.Y+ chunkRadius; y++)
				{
					var chunkPos = new Vector2(x, y);
					if (Vector2.Distance(chunkPos, center) < chunkRadius)
					{

						var chunk = World.GetChunk(center);

						TryOwnChunk(currentPlayer, chunkPos, chunkRadius);

						foundChunks.Add(new NetworkChunkPositionPair(chunk, center));
					}

				}
			}

			return foundChunks;
		}

		private void TryOwnChunk(RuntimePlayer player, Vector2 chunkPos, float ownRadius)
		{
			var chunk = World.GetChunk(chunkPos);

			var playerChunk = ChunkMath.WorldToChunkPos(player.Position);

			if (chunk.Owner == player.RuntimeId)
				return;
			if (chunk.Owner == -1)
			{
				World.UpdateChunkOwner(playerChunk, player.RuntimeId);
				return;
			}
			var oldPlayer = PlayerManager.GetPlayerById(chunk.Owner);
			if(oldPlayer == null)
			{
				World.UpdateChunkOwner(playerChunk, player.RuntimeId);
				return;
			}
			if (Vector2.Distance(chunkPos, ChunkMath.WorldToChunkPos(oldPlayer.Position)) >= ownRadius)
			{
				World.UpdateChunkOwner(playerChunk, player.RuntimeId);
				return;
			}



		}


		private List<NetworkPlayer> FindNetworkPlayersAroundPlayer(RuntimePlayer currentPlayer, int radius)
		{
			var foundPlayers = new List<NetworkPlayer>();
			var players = PlayerManager.Players.Values;
			foreach (var player in players)
			{
				if (ConfigLoader.Config.Debug == null || ConfigLoader.Config.Debug.ShowLocalPlayer == false)
				{
					if (player.RuntimeId == currentPlayer.RuntimeId)
						continue;

				}

				

				var dist = Vector3.Distance(player.Position, currentPlayer.Position);

				if (dist <= radius)
				{
					foundPlayers.Add(player.ToNetworkPlayer());
				}

			}
			return foundPlayers;
		}



		public void Stop()
		{


			_log.Info("Stopping server");
			NetManager.Stop(true);

		}

		
		private void OnJoinRequest(JoinRequestPacket packet, NetPeer peer)
		{
			var newRuntimePlayer = PlayerManager.CreateNewPlayer(packet.Username, peer.Id, packet.StaticPlayerId);


			_log.Info($"{packet.Username} joined the game");

			var playersAlreadyInGame = new List<InitialPlayerData>();
			foreach (var runtimePlayer in PlayerManager.Players.Values)
			{
				if (runtimePlayer.RuntimeId == peer.Id)
					continue;

				playersAlreadyInGame.Add(runtimePlayer.ToInitialPlayerData());
			}


			SendPacket(peer, new JoinAcceptPacket() 
			{ 
				Id = peer.Id, 
				Username = newRuntimePlayer.Username,
				Position = newRuntimePlayer.Position, 
				WorldSeed = World.Settings.Seed, 
				PlayersAlreadyInGame = playersAlreadyInGame, 
				
				ClientConfig = ConfigLoader.Config.Client.ToNetworkClientConfig(),

				Debug = ConfigLoader.Config.Debug != null,
				DebugConfig = ConfigLoader.Config.Debug.ToNetworkDebugConfig(), 
				
			}, DeliveryMethod.ReliableOrdered);

			var initialPlayerData = newRuntimePlayer.ToInitialPlayerData();
			SendPacketToAll(new PlayerJoinedPacket() { initialPlayerData = initialPlayerData }, DeliveryMethod.ReliableOrdered);
		}

		private void OnPlayerUpdate(PlayerUpdatePacket packet, NetPeer peer)
		{
			var player = PlayerManager.GetPlayerById(peer.Id);
			if (player == null)
			{
				_log.Error("Got update form non joined player");
				return;
			}

			//_log.Debug($"PLAYER UPDATE Position={packet.Position}; HeadPosition={packet.HeadPosition}; RHandPosition={packet.RHandPosition}; LHandPosition={packet.LHandPosition};");

			player.Position = packet.Position;
			player.HeadPosition = packet.HeadPosition;
			player.RHandPosition = packet.RHandPosition;
			player.LHandPosition = packet.LHandPosition;
		}




	}
}

