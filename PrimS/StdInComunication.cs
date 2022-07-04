using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimS
{
	public class StdInComunication : IDisposable
	{
		private Stream _stdin;


		public enum Command
		{
			None,
			ReloadConfig,
			ListPlayers,

		}



		public StdInComunication()
		{
			_stdin = Console.OpenStandardInput();
		}


		public void Update()
		{
			var data = _stdin.ReadByte();
			if (data == -1)
			{
				return;
			}
			var command = (Command)((byte)data);
			ReadCommand(command);

		}

		private string ReadCommand(Command command)
		{
			switch (command)
			{
				case Command.ReloadConfig:
					ConfigLoader.Load();
					break;

				default:
					break;
			}

			return null;
		}

		public void Dispose()
		{
			_stdin?.Dispose();
		}
	}

}
