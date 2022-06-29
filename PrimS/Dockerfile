FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
EXPOSE 9586/udp

COPY /bin/publish/net6.0 .
ENTRYPOINT ["./PrimS"]