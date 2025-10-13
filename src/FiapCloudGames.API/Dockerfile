FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ../FiapCloudGames.sln ./
COPY ../src/FiapCloudGames.API/FiapCloudGames.API.csproj src/FiapCloudGames.API/
COPY ../src/FiapCloudGames.Core/FiapCloudGames.Core.csproj src/FiapCloudGames.Core/
COPY ../src/FiapCloudGames.Infra/FiapCloudGames.Infra.csproj src/FiapCloudGames.Infra/
COPY ../src/FiapCloudGames.Test/FiapCloudGames.Test.csproj src/FiapCloudGames.Test/

RUN dotnet restore src/FiapCloudGames.API/FiapCloudGames.API.csproj

COPY . .

WORKDIR /src/src/FiapCloudGames.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish/. .

EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000
ENV DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "FiapCloudGames.API.dll"]
