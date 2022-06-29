#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
EXPOSE 9586/udp

COPY /PrimS/bin/publish/net6.0 .
ENTRYPOINT ["./PrimS"] 
