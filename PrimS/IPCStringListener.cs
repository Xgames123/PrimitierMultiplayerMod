using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierServer
{
	public class IPCStringListener : IDisposable
	{

		private FileSystemWatcher _watcher;
		private string _inFile;
		private string _outFile;

		public event Func<string, string?>? OnMessage;
		

		public IPCStringListener(string inFile, string outFile)
		{
			ChangePath(inFile, outFile);

		}

		public void ChangePath(string inFile, string outFile)
		{
			_watcher?.Dispose();
			_inFile = inFile;
			_watcher = new FileSystemWatcher(Path.GetDirectoryName(_inFile));
			_watcher.Changed += _watcher_Changed;

			_outFile = outFile;
		}

		private void _watcher_Changed(object sender, FileSystemEventArgs e)
		{
			if (e.FullPath != _inFile)
				return;
			if (e.ChangeType != WatcherChangeTypes.Changed)
				return;

			string command;
			try
			{
				command = File.ReadAllText(e.FullPath);
			}catch(Exception)
			{
				return;
			}
			if(command == null)
				return;
			var responce = OnMessage?.Invoke(command);
			if (responce == null)
				return;
			try
			{
				File.WriteAllText(_outFile, responce);
			}
			catch (Exception)
			{

			}


		}

		public void Dispose()
		{
			_watcher?.Dispose();
		}
	}

}
