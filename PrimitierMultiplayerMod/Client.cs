using LiteNetLib;
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

		public Client()
		{
			Listener = new EventBasedNetListener();
			NetManager = new NetManager(Listener);

			Listener.NetworkReceiveEvent += NetworkReceiveEvent;

			NetManager.Start();
		}

		private void NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
		{
			var packetId = (PacketId)reader.GetByte();

			switch (packetId)
			{
				case PacketId.SuccessfullyConnected:
					PMFLog.Message("Successfully connected to server");
					break;

			}


		}

		public void Connect(IPEndPoint endPoint)
		{
			var writer = new NetDataWriter();
			var packet = new ConnectionRequestPacket(PlayerInfo.Username, Application.version, Assembly.GetExecutingAssembly().GetName().Version.ToString());
			writer.PutPacket(packet);

			NetManager.Connect(endPoint, writer);
		}

		public void Update()
		{
			NetManager.PollEvents();
		}

		public void Stop()
		{
			NetManager.Stop();

		}

	}
}
