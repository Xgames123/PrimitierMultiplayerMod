# PrimitierMultiplayerMod

## Mod installation
1) Install [Primitier mod manager](https://github.com/Xgames123/PrimitierModManager)
2) Download the mod from the latest release.
3) Drag it into primitier mod manager and click the arrow to select it
4) Launch Primitier

## Server installation with docker
1) Download the docker image from the latest release.
2) On your server install docker
3) Run ```sudo docker load -i {path to the .tar file}```
4) Run ```sudo docker run --rm -d --network host prims -p 9543:9543/udp```

## primsconfig
PrimSConfig is a tool to configure prims while it is running