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

	}

	private void ConnectionRequestEvent(ConnectionRequest request)
	{
		var connectionRequestPacket = new ConnectionRequestPacket(request.Data);

		if (NetManager.ConnectedPeersCount >= ConfigLoader.Config.maxPlayers)
		{
			var writer = new NetDataWriter();

			writer.Put((byte)shared.ErrorCode.ServerFull);
			writer.Put("The server is full");

			request.Reject(writer);
		}



		var isSupported = SupportedVersions.SupportedModVersions.Contains(Version.Parse(connectionRequestPacket.MultiplayerModVersion));
		if (!isSupported)
		{
			var writer = new NetDataWriter();
			
			writer.Put((byte)shared.ErrorCode.UnsupportedModVersion);
			writer.Put("The version of Primitier multilayer mod was not supported by the server");

			request.Reject(writer);
			return;
		}

		PlayerManager.CreateNewPlayer(connectionRequestPacket.Username, request.);

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
