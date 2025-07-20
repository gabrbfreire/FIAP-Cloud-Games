FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/FiapCloudGames.API/FiapCloudGames.API.csproj", "src/FiapCloudGames.API/"]
COPY ["src/FiapCloudGames.Application/FiapCloudGames.Application.csproj", "src/FiapCloudGames.Application/"]
COPY ["src/FiapCloudGames.Core/FiapCloudGames.Core.csproj", "src/FiapCloudGames.Core/"]
COPY ["src/FiapCloudGames.Infra/FiapCloudGames.Infra.csproj", "src/FiapCloudGames.Infra/"]

RUN dotnet restore src/FiapCloudGames.API/FiapCloudGames.API.csproj

COPY . .

WORKDIR /src/src/FiapCloudGames.API
RUN dotnet build FiapCloudGames.API.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish FiapCloudGames.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FiapCloudGames.API.dll"]