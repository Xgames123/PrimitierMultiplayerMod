using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierServer.shared
{
	public enum ErrorCode
	{
		ProtocolError,
		Unknown,
		ServerFull,
		UnsupportedModVersion,
	}
}
