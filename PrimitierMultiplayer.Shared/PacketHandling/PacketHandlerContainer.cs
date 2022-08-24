using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PrimitierMultiplayer.Shared.PacketHandling
{
	public class PacketHandlerContainer
	{
		public NetManager NetManager;
		public NetPacketProcessor NetPacketProcessor;
		public NetDataWriter Writer;

		private List<PacketHandler> PacketHandlers = new List<PacketHandler>();


		public PacketHandlerContainer(ref NetManager netManager)
		{
			NetManager = netManager;
			NetPacketProcessor = new NetPacketProcessor();
			Writer = new NetDataWriter();
		}
		public PacketHandlerContainer(ref NetManager netManager, ref NetPacketProcessor netPacketProcessor, ref NetDataWriter writer)
		{
			NetManager = netManager;
			NetPacketProcessor = netPacketProcessor;
			Writer = writer;
		}


		public void AddPacketHandlers(Assembly assembly)
		{

			var packetHandlerType = typeof(PacketHandler);
			foreach (var type in assembly.GetExportedTypes())
			{
				if (!type.IsAbstract && !type.IsGenericType && packetHandlerType.IsAssignableFrom(type))
				{
					var packetHandler = (PacketHandler)Activator.CreateInstance(type);
					if (packetHandler == null)
						continue;
					packetHandler.Setup(ref Writer, ref NetPacketProcessor, ref NetManager);
					PacketHandlers.Add(packetHandler);
				}

			}

		}

		public void ReadAllPackets(NetDataReader reader, NetPeer peer)
		{
			NetPacketProcessor.ReadAllPackets(reader, peer);
		}


	}
}
