using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimS.shared
{

	public static class NetDataWriterExtentions
	{
		public static void PutPacket<T>(this NetDataWriter writer, T packet) where T : Packet
		{
			packet.PutOnWriter(ref writer);
		}

	}

}

