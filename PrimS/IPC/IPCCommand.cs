using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierServer.IPC
{

	public enum IPCCommandType
	{
		Invalid,
		ReloadConfig,
		ListPlayers,
		ReloadWorld,
		ClearChunkCache,
	}

	public enum IPCResponceType
	{
		InvalidCommand,
		Error,
		Ok,

	}

	public class IPCCommand
	{
		public IPCCommandType Type { get; set; }

	}
	public class IPCResponce
	{
		public IPCResponceType Type { get; set; }
		public object? Data { get; set; }

		private static IPCResponce c_invalidCommand = new IPCResponce() { Type = IPCResponceType.InvalidCommand, Data = null };
	

		public static IPCResponce Ok(object? data)
		{
			return new IPCResponce() { Type = IPCResponceType.Ok, Data = data };
		}
		public static IPCResponce Ok()
		{
			return Ok(null);
		}
		public static IPCResponce Error()
		{
			return Error(null);
		}
		public static IPCResponce Error(string? message)
		{
			return new IPCResponce() { Type = IPCResponceType.Error, Data = message };
		}
		
		public static IPCResponce InvalidCommand()
		{
			return c_invalidCommand;
		}
	}
}
