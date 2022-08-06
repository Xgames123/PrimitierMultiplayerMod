using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierServer.Shared
{
	public enum ErrorCode
	{
		ProtocolError,
		Unknown,
		ServerFull,
		ModVersionToLow,
		UnsupportedModVersion,
	}
}
