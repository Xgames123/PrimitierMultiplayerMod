using LiteNetLib;
using PrimitierMultiplayer.Shared.PacketHandling;
using PrimitierMultiplayer.Shared.Packets.s2c;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Mod.PacketHandlers
{
	public class ServerUpdatePacketHandler : PacketHandler<ServerUpdatePacket>
	{
		public override void HandelPacket(ServerUpdatePacket packet, NetPeer peer)
		{

			foreach (var networkPlayer in packet.Players)
			{
				var remotePlayer = RemotePlayer.RemotePlayers[networkPlayer.Id];
				//PMFLog.Message($"NET PLAYER Position={networkPlayer.Position}; Position={networkPlayer.HeadPosition};");
				remotePlayer.Sync(networkPlayer);

			}

			//PMFLog.Message("Got server update");
			//var testData = new PrimitierServer.Shared.NetworkChunk() { Cubes = new System.Collections.Generic.List<PrimitierServer.Shared.NetworkCube>() { new PrimitierServer.Shared.NetworkCube() { Id = 1, Position = new System.Numerics.Vector3(0, 0, 0), Size = new System.Numerics.Vector3(5, 5, 5), Substance = 0, Rotation = new System.Numerics.Quaternion(0, 0, 0, 0) } } };
			//PMFLog.Message(JSON.Dump(testData));
			//PMFLog.Message(packet.Chunks.Length);
			//PMFLog.Message(JSON.Dump(packet.Chunks[0]));


			WorldManager.UpdateModChunks(packet.Chunks);

		}
	}
}
