using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared.Packets
{
	public class ErrorPacket
	{
		public ErrorCode ErrorCode;
		public string Message = null;

	}
}
