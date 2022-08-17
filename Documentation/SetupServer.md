# Setting up a server
**Use a vpn if you are going to be running a server on your local machine**

[I recommend radmin vpn](https://www.radmin-vpn.com/)

## installation without docker
1) Download PrimitierServer from the release you want to install
2) install .Net 5 runtime (When you run the application it will give you a download link)
3) unpack the zip file
4) edit `primsconfig.json` see [Configure the server](#configure-the-server)
5) run the main executable (You probably need to chmod it first when on linux)

## installation with docker
1) install git and docker
2) ```git clone https://github.com/Xgames123/PrimitierMultiplayerMod.git --recursive```
3) edit `primsconfig.json` see [Configure the server](#configure-the-server)
3) Run ```sudo docker build . -t multiplayerserver```
4) Run ```sudo docker run --rm -d --network host multiplayerserver -p 9543:9543/udp```


## Configure the server
Settings for the server are stored in primsconfig.json
```json
{

	"ListenPort": 9543, //Port to listen on
	"ListenIp": "0.0.0.0", //Ip address to use for the server

	"MaxPlayers": 10, //Maximum players that can be in the world
	"WorldDirectory": "World", //The path to the directory to store world data into
	"UpdateDelay": 100, //Amount of milliseconds between server updates (When the server sends a packet to all connected clients)
	"IPCDirectory": null, //Directory to put the files to communicate with the running instance of the server (Best to put it in a virtual file system)

	//Configurations for the clients that connect
	"Client": {
		"IdleUpdateDelay": 1000, //Amount of milliseconds between client updates when the client is idle (when the client is too far away from other players to be seen)
		"ActiveUpdateDelay": 20 //Amount of milliseconds between client updates when the client can be seen by other players
	},

	//You should probably remove this section
	//Configurations for debugging the mod and server
	"Debug": {
		"Debug": false, //If false disable all other settings in this section

		"ShowLocalPlayer": false //If true makes the server send the local player back to itself
	}
}
```