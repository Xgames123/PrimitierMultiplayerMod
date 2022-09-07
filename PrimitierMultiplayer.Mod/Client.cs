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
	public class Client : Peer
	{

		public bool IsConnected { get { return NetManager.ConnectedPeersCount >= 1; } }

		public NetPeer Server { get; private set; } = null;

		public event Action<NetPeer> OnDisconnectFromServer;

		private Stopwatch _updateStopwatch = Stopwatch.StartNew();

		public Client() : base()
		{
			PacketHandlerContainer.AddPacketHandlers(Assembly.GetExecutingAssembly());
		}

		public void Connect(string address, int port)
		{
			if (IsConnected)
				Disconnect();


			Mod.Chat.AddSystemMessage("Connecting to the server");

			if (!NetManager.IsRunning)
			{
				PMFLog.Message("Starting client");
				NetManager.Start();
			}
			

			PMFLog.Message($"Connecting to {address}:{port}...");

			Writer.Reset();
			Writer.Put(Mod.ModVersion.ToString());
			NetManager.Connect(address, port, Writer);
		}

		public void Disconnect()
		{
			NetManager.DisconnectAll();
			PMFLog.Message("Disconnecting from server");
		}

		public override void Update()
		{
			base.Update();

			if (!IsRunning)
				return;

			if (MultiplayerManager.IsInMultiplayerMode && IsConnected)
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

			List<NetworkChunkPositionPair> sendChunks = new List<NetworkChunkPositionPair>();

			PMFLog.Message($"sending chunks");
			foreach (var chunkEntry in WorldManager.VisibleChunks)
			{
				if(chunkEntry.Value.Owner == MultiplayerManager.LocalId)
				{
					var runtimeChunk = chunkEntry.Value;

					PMFLog.Message($"Pos: X: {chunkEntry.Key.X}, Y: {chunkEntry.Key.Y}");
					PMFLog.Message($"Owner: {runtimeChunk.Owner}");
					PMFLog.Message($"cubeCount: {runtimeChunk.NetworkSyncs.Count}");

					var netChunk = runtimeChunk.UpdateToServer();
					sendChunks.Add(new NetworkChunkPositionPair(netChunk, chunkEntry.Key));
				}

			}


			var packet = new PlayerUpdatePacket()
			{
				Position = PMFHelper.CameraRig.transform.position.ToNumerics(),
				HeadPosition = Camera.main.transform.position.ToNumerics(),
				LHandPosition = PMFHelper.LHand.transform.position.ToNumerics(),
				RHandPosition = PMFHelper.RHand.transform.position.ToNumerics(),

				Chunks = sendChunks.ToArray(),
			};


			SendPacket(Server, packet, DeliveryMethod.Unreliable);

		}

		public void Start()
		{
			NetManager.Start();
			PMFLog.Message($"Started client");
		}

		public override void Stop()
		{
			Disconnect();
			base.Stop();
		}


		protected override void PeerConnectedEvent(NetPeer peer)
		{
			Server = peer;
			PMFLog.Message("Connected to the server");

			SendPacket(Server, new JoinRequestPacket() { Username = PlayerInfo.Username, StaticPlayerId = PlayerInfo.StaticId }, DeliveryMethod.ReliableOrdered);
		}

		protected override void PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
		{
			OnDisconnectFromServer?.Invoke(Server);
			
			Server = null;
			ReadErrorFromDisconnectInfo(disconnectInfo);
			

			Mod.Chat.AddSystemMessage($"Disconnected from the server Reason:{disconnectInfo.Reason}");
			
		}



	}
}
