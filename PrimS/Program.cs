﻿using PrimS;

ConfigLoader.Load();

var comunication = new StdInComunication();

var server = new Server();

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs e) =>
{
	cts.Cancel();
};


while (!cts.Token.IsCancellationRequested)
{
	server.Update();
	comunication.Update();
}

server.Stop();
comunication.Dispose();