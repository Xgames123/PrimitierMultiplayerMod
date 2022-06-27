using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimS.shared
{
	public static class ErrorGenerator
	{

		public static Dictionary<ErrorCode, string> ErrorMessages = new Dictionary<ErrorCode, string>()
		{
			{ErrorCode.Unknown, "Unknown error"},
			{ErrorCode.ServerFull, "The server is full"},
			{ErrorCode.UnsupportedModVersion, "The version of Primitier multilayer mod was not supported by the server"},

		};



		public static NetDataWriter Generate(ErrorCode code)
		{
			var writer = new NetDataWriter();
			Generate(ref writer, code);
			return writer;
		}

		public static void Generate(ref NetDataWriter writer, ErrorCode code)
		{
			writer.Put((byte)code);
			writer.Put(ErrorMessages[code]);
		}
	}
}
