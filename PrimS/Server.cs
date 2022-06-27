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

	public Server()
	{
		Listener = new EventBasedNetListener();
		NetManager = new NetManager(Listener);

		Listener.ConnectionRequestEvent += ConnectionRequestEvent;
		Listener.PeerConnectedEvent += PeerConnectedEvent;

		ConfigLoader.OnConfigReload += OnConfigReload;
		OnConfigReload(ConfigLoader.Config);		
	}

	private void PeerConnectedEvent(NetPeer peer)
	{
		var player = PlayerManager.GetPlayerByIpAddress(peer.EndPoint);

		var writer = new NetDataWriter();
		if (player == null)
		{
			ErrorGenerator.Generate(ref writer, shared.ErrorCode.Unknown);
			peer.Disconnect(writer);
			return;
		}

		writer.PutPacket(new SuccessfullyConnectedPacket(player.Id, player.Username, player.Position));

		peer.Send(writer, DeliveryMethod.ReliableUnordered);
	}

	private void ConnectionRequestEvent(ConnectionRequest request)
	{
		var connectionRequestPacket = new ConnectionRequestPacket(request.Data);

		if (NetManager.ConnectedPeersCount >= ConfigLoader.Config.maxPlayers)
		{
			request.Reject(ErrorGenerator.Generate(shared.ErrorCode.ServerFull));
			return;
		}



		var isSupported = SupportedVersions.SupportedModVersions.Contains(Version.Parse(connectionRequestPacket.MultiplayerModVersion));
		if (!isSupported)
		{

			request.Reject(ErrorGenerator.Generate(shared.ErrorCode.UnsupportedModVersion));
			return;
		}

		PlayerManager.CreateNewPlayer(connectionRequestPacket.Username, request.RemoteEndPoint.Address);

		request.Accept();
		
	}

	private void OnConfigReload(ConfigFile? config)
	{
		if (config == null)
		{
			_log.Error("Config was null. Stopping server");
			Stop();
			return;
		}
		
		if (!NetManager.IsRunning)
		{
			NetManager.Start(config.ListenPort);
		}
		
	}

	public void Update()
	{
		if (!NetManager.IsRunning)
			return;

		NetManager.PollEvents();

	}


	public void Stop()
	{
		NetManager.Stop();

	}

	



}
