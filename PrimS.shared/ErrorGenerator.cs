using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimS.shared
{
	public static class ErrorGenerator
	{
		public static void Generate(ref NetDataWriter writer, ErrorCode code)
		{
			writer.Put((byte)code);

		}
	}
}
