FROM mcr.microsoft.com/dotnet/runtime:6.0-bullseye-slim-armv7
WORKDIR /app
EXPOSE 9543/udp

COPY PrimS/bin/publish/net6.0 .
RUN chmod +x ./PrimS
ENTRYPOINT ["./PrimS"]