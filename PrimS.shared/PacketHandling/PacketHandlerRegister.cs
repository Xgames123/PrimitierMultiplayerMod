using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PrimitierMultiplayer.Shared.PacketHandling
{
	public class PacketHandlerRegister
	{
		public NetManager NetManager;
		public NetPacketProcessor NetPacketProcessor;
		public NetDataWriter Writer;

		private List<PacketHandler> PacketHandlers = new List<PacketHandler>();


		public PacketHandlerRegister(ref NetManager netManager)
		{
			NetManager = netManager;
			NetPacketProcessor = new NetPacketProcessor();
			Writer = new NetDataWriter();
		}
		public PacketHandlerRegister(ref NetManager netManager, ref NetPacketProcessor netPacketProcessor, ref NetDataWriter writer)
		{
			NetManager = netManager;
			NetPacketProcessor = netPacketProcessor;
			Writer = writer;
		}


		public void AddPacketHandlers(Assembly assembly, ref NetDataWriter writer, ref NetPacketProcessor packetProcessor, ref NetManager netManager)
		{

			foreach (var type in assembly.GetExportedTypes())
			{
				if (!type.IsGenericType && type.IsAssignableFrom(typeof(PacketHandler)))
				{
					var packetHandler = (PacketHandler)Activator.CreateInstance(type);
					if (packetHandler == null)
						continue;
					packetHandler.Setup(ref writer, ref packetProcessor, ref netManager);
					PacketHandlers.Add(packetHandler);
				}

			}

		}

		public void ReadAllPackets(NetDataReader reader, NetPeer peer)
		{
			NetPacketProcessor.ReadAllPackets(reader);
		}


	}
}
