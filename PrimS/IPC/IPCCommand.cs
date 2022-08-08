using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierServer.IPC
{

	public enum IPCCommandType
	{

		RELOAD_CONF,
		LS_PLAYERS,
		RELOAD_WORLD,
		CLEAR_CHUNK_CACHE,
	}

	public enum IPCResponceType
	{
		INVALID_COMMAND,
		ERROR,
		OK,

	}

	public class IPCCommand
	{
		public IPCCommandType Type;

	}
	public class IPCResponce
	{
		public IPCResponceType Type;
		public object? Data;

		private static IPCResponce c_invalidCommand = new IPCResponce() { Type = IPCResponceType.INVALID_COMMAND, Data = null };
	

		public static IPCResponce Ok(object? data)
		{
			return new IPCResponce() { Type = IPCResponceType.OK, Data = data };
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
			return new IPCResponce() { Type = IPCResponceType.ERROR, Data = message };
		}
		
		public static IPCResponce InvalidCommand()
		{
			return c_invalidCommand;
		}
	}
}
