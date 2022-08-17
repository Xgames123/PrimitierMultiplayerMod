# Setting up a server
**Use a vpn if you are going to be running a server on your local machine**

[I recommend radmin vpn](https://www.radmin-vpn.com/)

## installation without docker
1) Download PrimitierServer from the release you want to install
2) install .Net 5 runtime (When you run the application it will give you a download link)
3) unpack the zip file
4) edit `primsconfig.json` see [primsconfig.json](../PrimS/primsconfig.json)
5) run the main executable (You probably need to chmod it first when on linux)

## installation with docker
1) install git and docker
2) ```git clone https://github.com/Xgames123/PrimitierMultiplayerMod.git --recursive```
3) edit `primsconfig.json` see [primsconfig.json](../PrimS/primsconfig.json)
3) Run ```sudo docker build . -t multiplayerserver```
4) Run ```sudo docker run --rm -d --network host multiplayerserver -p 9543:9543/udp```


## Configure the server
Settings for the server are stored in [primsconfig.json](../PrimS/primsconfig.json)