using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using PrimitierMultiplayer.Shared.Models;
using PrimitierMultiplayer.Shared.Models.Config;

namespace PrimitierMultiplayer.Shared
{
	public static class PacketProcessorTypeRegister
	{



		public static void RegisterNetworkModels(ref NetPacketProcessor packetProcessor)
		{
			packetProcessor.RegisterNestedType<NetworkClientConfig>();
			packetProcessor.RegisterNestedType<NetworkDebugConfig>();

			packetProcessor.RegisterNestedType<NetworkChunkPositionPair>();
			packetProcessor.RegisterNestedType<NetworkChunk>();
			packetProcessor.RegisterNestedType<NetworkCube>();
			packetProcessor.RegisterNestedType<NetworkPlayer>();
			packetProcessor.RegisterNestedType<InitialPlayerData>();
			packetProcessor.RegisterNestedType((writer, value) => writer.Put(value), reader => reader.GetVector3());
			packetProcessor.RegisterNestedType((writer, value) => writer.PutList(value), reader => reader.GetList<InitialPlayerData>());
		}
	}
}
