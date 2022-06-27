using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib;
using LiteNetLib.Utils;

namespace PrimS.shared
{
	public abstract class Packet
	{
		public abstract PacketId PacketId { get; }

		public Packet(NetDataReader reader){}
		public Packet() { }

		public abstract void PutOnWriter(ref NetDataWriter writer);
	}
}
