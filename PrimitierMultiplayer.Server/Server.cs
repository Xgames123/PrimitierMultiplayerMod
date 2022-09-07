using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;
using log4net;
using PrimitierMultiplayer.Shared.Packets.c2s;
using PrimitierMultiplayer.Shared.Packets.s2c;
using PrimitierMultiplayer.Shared;
using System.Numerics;
using System.Diagnostics;
using PrimitierMultiplayer.Server.Mappers;
using PrimitierMultiplayer.Server.WorldStorage;
using PrimitierMultiplayer.Shared.Models;
using PrimitierMultiplayer.Shared.PacketHandling;
using System.Reflection;

namespace PrimitierMultiplayer.Server
{
	public class Server : Peer
	{
		
		
		private ILog _log = LogManager.GetLogger(nameof(Server));


		private Stopwatch _updateStopwatch = Stopwatch.StartNew();

		public Server() : base()
		{
			
			PacketHandlerContainer.AddPacketHandlers(Assembly.GetExecutingAssembly());

			
		}





		protected override void PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
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



		protected override void ConnectionRequestEvent(ConnectionRequest request)
		{
			_log.Info(request.RemoteEndPoint + " has requested a connection");


			if (NetManager.ConnectedPeersCount >= ConfigLoader.Config.MaxPlayers)
			{
				RejectConnectionRequest(request, ErrorCode.ServerFull);
				return;
			}

			var reader = request.Data;
			Version? modVersion = null;
			if (!Version.TryParse(reader.GetString(), out modVersion))
			{
				RejectConnectionRequest(request, ErrorCode.ProtocolError);
				return;
			}
			if (!SupportedVersions.CheckModVersion(modVersion))
			{
				RejectConnectionRequest(request, ErrorCode.UnsupportedModVersion);
			}


			request.Accept();

		}


		public override void Update()
		{
			base.Update();
			if (!IsRunning)
				return;

			if (ConfigLoader.Config == null)
				return;

			if (_updateStopwatch.ElapsedMilliseconds >= ConfigLoader.Config.UpdateDelay)
			{
				_updateStopwatch.Restart();

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

				var players = FindNetworkPlayersAroundPlayer(currentPlayer, ConfigLoader.Config.ViewRadius);

				var chunks = FindNetworkChunksAroundPlayer(currentPlayer, ConfigLoader.Config.ViewRadius);

				SendPacket(peer, new ServerUpdatePacket() { Players = players.ToArray(), Chunks = chunks.ToArray() }, DeliveryMethod.Unreliable);

			}

		}


		private List<NetworkChunkPositionPair> FindNetworkChunksAroundPlayer(RuntimePlayer currentPlayer, int chunkRadius)
		{
			var foundChunks = new List<NetworkChunkPositionPair>();


			var center = ChunkMath.WorldToChunkPos(currentPlayer.Position);

			if(chunkRadius == 0)
			{
				var chunk = World.GetChunk(center);
				World.TryOwnChunk(currentPlayer, center, chunkRadius);
				foundChunks.Add(new NetworkChunkPositionPair(chunk, center));
				
				return foundChunks;
			}


			for (float x = center.X - chunkRadius; x < center.X + chunkRadius; x++)
			{
				for (float y = center.Y - chunkRadius; y < center.Y + chunkRadius; y++)
				{
					var chunkPos = new Vector2(x, y);
					if (Vector2.Distance(chunkPos, center) < chunkRadius)
					{

						var chunk = World.GetChunk(chunkPos);

						World.TryOwnChunk(currentPlayer, chunkPos, chunkRadius);

						foundChunks.Add(new NetworkChunkPositionPair(chunk, center));
					}

				}
			}

			return foundChunks;
		}

		private List<NetworkPlayer> FindNetworkPlayersAroundPlayer(RuntimePlayer currentPlayer, int chunkRadius)
		{
			var foundPlayers = new List<NetworkPlayer>();
			var players = PlayerManager.Players.Values;

			var radius = ChunkMath.ChunkToWorldRadius(chunkRadius);

			foreach (var player in players)
			{
				if (ConfigLoader.Config.Debug == null || ConfigLoader.Config.Debug.ShowLocalPlayer == false)
				{
					if (player.RuntimeId == currentPlayer.RuntimeId)
						continue;

				}



				var dist = Vector3.Distance(player.Position, currentPlayer.Position);

				if (dist <= radius)
					foundPlayers.Add(player.ToNetworkPlayer());

			}
			return foundPlayers;
		}

		public void Start(ConfigFile configFile)
		{
			Start(configFile.ListenIp, configFile.ListenPort);
		}

		public void Start(string listenIp, int listenPort)
		{
			NetManager.Start(listenIp, "", listenPort);
			_log.Info($"Started server on {listenIp}:{listenPort}");
		}

		public override void Stop()
		{
			_log.Info("Stopping server");
			base.Stop();
		}







	}
}

