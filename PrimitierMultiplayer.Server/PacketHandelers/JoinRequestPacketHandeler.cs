using LiteNetLib;
using LiteNetLib.Utils;
using log4net;
using PrimitierMultiplayer.Server.Mappers;
using PrimitierMultiplayer.Server.WorldStorage;
using PrimitierMultiplayer.Shared.Models;
using PrimitierMultiplayer.Shared.PacketHandling;
using PrimitierMultiplayer.Shared.Packets.c2s;
using PrimitierMultiplayer.Shared.Packets.s2c;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Server.PacketHandelers
{
	public class JoinRequestPacketHandler : PacketHandler<JoinRequestPacket>
	{
		private ILog _log = LogManager.GetLogger(nameof(JoinRequestPacketHandler));


		public override void HandelPacket(JoinRequestPacket packet, NetPeer peer)
		{
			var newRuntimePlayer = PlayerManager.CreateNewPlayer(packet.Username, peer.Id, packet.StaticPlayerId);


			_log.Info($"{packet.Username} joined the game");
#if DEBUG
			_log.Debug($"Spawning player at position X: {newRuntimePlayer.Position.X} Y: {newRuntimePlayer.Position.Y} Z: {newRuntimePlayer.Position.Z}");
#endif

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

	}
}
