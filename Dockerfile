FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy everything
COPY PrimS/. ./PrimS
COPY PrimS.shared/. ./PrimS.shared

# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
WORKDIR /app/PrimS
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build-env /app/PrimS/out .
ENTRYPOINT ["dotnet", "PrimS.dll"]

