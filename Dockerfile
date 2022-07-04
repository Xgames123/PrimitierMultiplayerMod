FROM mcr.microsoft.com/dotnet/runtime:6.0-bullseye-slim-arm32v7
WORKDIR /app
COPY ./PrimS/bin/publish/net6.0 .

EXPOSE 9543/udp
ENTRYPOINT ["dotnet" "PrimS.dll"]
