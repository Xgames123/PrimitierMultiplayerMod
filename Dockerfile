FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy everything
COPY PrimS/. ./PrimS
COPY PrimS.shared/. ./PrimS.shared
COPY LiteNetLib/. ./LiteNetLib

#PrimS.shared
WORKDIR /app/PrimS.shared

# Restore as distinct layers
RUN dotnet restore

#PrimS
WORKDIR /app/PrimS

# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out




# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build-env /app/PrimS/out .
EXPOSE 9543/udp
ENTRYPOINT ["dotnet", "PrimS.dll"]

