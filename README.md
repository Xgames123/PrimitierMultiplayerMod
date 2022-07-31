# PrimitierMultiplayerMod
**This is in development so the main branch doens't always work**

## Mod installation
1) Install [Primitier mod manager](https://github.com/Xgames123/PrimitierModManager)
2) Download the mod from the latest release.
3) Drag it into primitier mod manager and click the arrow to select it
4) Launch Primitier

## Server installation with docker
1) install git and docker
2) git clone https://github.com/Xgames123/PrimitierMultiplayerMod.git
3) edit primsconfig.json
3) Run ```sudo docker build . -t multiplayerserver```
4) Run ```sudo docker run --rm -d --network host multiplayerserver -p 9543:9543/udp```

## primsconfig.json
primsconfig.json is the configuration file for the server
```json
{
	"ListenPort": 9543, //Port to listen on
	"ListenIp": "localhost", //Ip address to use for the server

	"MaxPlayers": 10, //Maximum players that can be in the world
	"WorldDirectory": "World", //The path to the directory to store world data into
    "UpdateDelay": 10, //Amount of milliseconds between server updates (When the server sends a packet to all connected clients)

    //Configurations for the clients that connect
    "Client": {
        "IdleUpdateDelay": 1000, //Amount of milliseconds between client updates when the client is idle (when the client is too far away from other players to be seen)
        "ActiveUpdateDelay": 20 //Amount of milliseconds between client updates when the client can be seen by other players
    }
}
```
