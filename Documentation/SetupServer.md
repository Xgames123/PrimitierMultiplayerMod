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
Settings for the server are stored in a file with the name primsconfig.json

Note: You don't have to set every property this is just to show you witch properties there are
```json
{

	"ListenPort": 9543, //Port to listen on
	"ListenIp": "0.0.0.0", //Ip address to use for the server
	
	"IPCDirectory": null, //Directory to put the files to communicate with the running instance of the server (Best to put it in a virtual file system)

	"MaxPlayers": 10, //Maximum players that can be in the world
	"UpdateDelay": 100, //Amount of milliseconds between server updates (When the server sends a packet to all connected clients)
	
	
	//Configurations for the clients that connect
	"Client": {
		"IdleUpdateDelay": 1000, //Amount of milliseconds between client updates when the client is idle (when the client is too far away from other players to be seen)
		"ActiveUpdateDelay": 20 //Amount of milliseconds between client updates when the client can be seen by other players
	},
	
	
	"WorldDirectory": "World", //The path to the directory to store world data into
	"MaxChunkCacheSize": 4000, //The maximum amount of chunks that can be cached (If the cache size exceeds this value it will clear the entire cache so don't set this too small)
	"ViewRadius": 4, //The amount of chunks every player can see
	
	
	//You should probably remove this section
	//Configurations for debugging the mod and server
	"Debug": {
		"Debug": false, //If false disable all other settings in this section

		"ShowLocalPlayer": false //If true makes the server send the local player back to itself
		"ShowChunkBounds": false //If true shows the chunk bounds on every player (Can also be toggled by pressing F4)
	}
}
```