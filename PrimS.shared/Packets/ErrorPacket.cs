using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierServer.Shared.Packets
{
	public class ErrorPacket
	{
		public ErrorCode ErrorCode;
		public string? Message = null;

	}
}
