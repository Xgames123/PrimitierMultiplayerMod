using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimS.shared
{

	public static class NetDataWriterExtentions
	{
		public static void PutPacket<T>(this NetDataWriter writer, T packet) where T : Packet
		{
			writer.Put((byte)packet.PacketId);
			packet.PutOnWriter(ref writer);
		}

		public static void PutVector3(this NetDataWriter writer, Vector3 vector3)
		{
			writer.Put(vector3.X);
			writer.Put(vector3.Y);
			writer.Put(vector3.Z);
		}

	}

}

