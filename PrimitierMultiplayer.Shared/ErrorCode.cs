using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared
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
