using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;
using log4net;
using log4net.Core;
using PrimS.shared.Packets;
using PrimS.shared.Packets.c2s;
using PrimS.shared.Packets.s2c;
using PrimS.shared;

namespace PrimS;
public class Server
{
	public EventBasedNetListener Listener;
	public NetManager NetManager;

	public bool IsStarted { get; private set; } = false;

	private ILog _log = LogManager.GetLogger(nameof(Server));

	private NetPacketProcessor _packetProcessor;
	private NetDataWriter _writer;

	public Server()
	{
		Listener = new EventBasedNetListener();
		NetManager = new NetManager(Listener)
		{
			AutoRecycle = true
		};

		Listener.ConnectionRequestEvent += ConnectionRequestEvent;
		Listener.PeerDisconnectedEvent += PeerDisconnectedEvent;
		Listener.NetworkErrorEvent += NetworkErrorEvent;
		Listener.NetworkReceiveEvent += NetworkReceiveEvent;

		_writer = new NetDataWriter();
		_packetProcessor = new NetPacketProcessor();
		_packetProcessor.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetVector3());
		_packetProcessor.SubscribeReusable<JoinRequestPacket, NetPeer>(OnJoinRequest);


		ConfigLoader.OnConfigReload += OnConfigReload;
		OnConfigReload(ConfigLoader.Config);		
	}

	private void NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
	{
		_packetProcessor.ReadAllPackets(reader);
	}

	private void OnConfigReload(ConfigFile? config)
	{
		if (config == null)
		{
			_log.Error("Config was null. Ignoring... reload");
			return;
		}
		Stop();

		NetManager.Start(config.ListenIp, "", config.ListenPort);
		_log.Info($"Started server on {config.ListenIp}:{NetManager.LocalPort}");
		

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

		PlayerManager.DeletePlayer(player.Id);
	}



	private void ConnectionRequestEvent(ConnectionRequest request)
	{
		_log.Info(request.RemoteEndPoint+" has requested a connection");
		

		if (NetManager.ConnectedPeersCount >= ConfigLoader.Config.maxPlayers)
		{
			_writer.Reset();
			ErrorGenerator.Generate(ref _writer, ref _packetProcessor, shared.ErrorCode.ServerFull);
			request.Reject(_writer);
			return;
		}

		//TODO check for unsupported versions

		request.Accept();
		
	}


	public void Update()
	{
		if (!NetManager.IsRunning)
			return;

		NetManager.PollEvents();

	}

	public void Stop()
	{
		if (!NetManager.IsRunning)
		{
			return;
		}

		_log.Info("Stopping server");
		NetManager.Stop();

	}

	private void OnJoinRequest(JoinRequestPacket packet, NetPeer peer)
	{
		PlayerManager.CreateNewPlayer(packet.Username, peer.Id);
		_log.Info($"{packet.Username} joined the game");
	}






}
