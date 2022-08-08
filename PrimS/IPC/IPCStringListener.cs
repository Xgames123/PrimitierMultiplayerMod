using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierServer.IPC
{
	public class IPCStringListener : IDisposable
	{

		private FileSystemWatcher _watcher;
		private string _inFile;
		private string _outFile;
		private static ILog s_log = LogManager.GetLogger(nameof(IPCStringListener));

		public const string OutFileName = "PRIMITIERSERVER.cmdout";
		public const string InFileName = "PRIMITIERSERVER.cmdin";
		public event Func<string, string?>? OnMessage;


		public IPCStringListener(string path)
		{
			ChangePath(path);
			s_log.Info($"Started listening for data in file '{_inFile}'");
		}

		public void ChangePath(string path)
		{
			_watcher?.Dispose();
			_inFile = Path.Combine(path, InFileName);
			_watcher = new FileSystemWatcher(path);
			_watcher.NotifyFilter = NotifyFilters.LastWrite;
			_watcher.Filter = "";
			_watcher.Changed += _watcher_Changed;
			_watcher.EnableRaisingEvents = true;

			_outFile = Path.Combine(path, OutFileName);

			try
			{
				File.WriteAllText(_inFile, "");
				File.WriteAllText(_outFile, "");
			}
			catch (Exception e)
			{
				s_log.Error("Could not create IPC files", e);
			}

		}

		private void _watcher_Changed(object sender, FileSystemEventArgs e)
		{
			if (e.FullPath != _inFile)
				return;

			string command;
			try
			{
				command = File.ReadAllText(e.FullPath);
			}
			catch (Exception)
			{
				return;
			}
			if (command == null)
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
