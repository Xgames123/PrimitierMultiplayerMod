using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierServer.Shared
{
	public static class PacketProcessorTypeRegister
	{
		public static void RegisterNetworkTypes(ref NetPacketProcessor packetProcessor)
		{

			packetProcessor.RegisterNestedType<NetworkChunk>();
			packetProcessor.RegisterNestedType<NetworkCube>();
			packetProcessor.RegisterNestedType<NetworkPlayer>();
			packetProcessor.RegisterNestedType<InitialPlayerData>();
			packetProcessor.RegisterNestedType((writer, value) => writer.Put(value), reader => reader.GetVector3());
			packetProcessor.RegisterNestedType((writer, value) => writer.PutList(value), reader => reader.GetList<InitialPlayerData>());
		}
	}
}
