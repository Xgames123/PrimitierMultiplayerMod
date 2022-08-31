using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared.Packets
{
	public class ErrorPacket : Packet
	{
		public ErrorCode ErrorCode;
		public string Message = null;

	}
}
