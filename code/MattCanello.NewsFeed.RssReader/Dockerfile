#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

HEALTHCHECK --interval=35s --timeout=4s CMD curl --fail http://localhost:80/app/health || exit 1

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./code/MattCanello.NewsFeed.Cross.Dapr/MattCanello.NewsFeed.Cross.Dapr.csproj", "MattCanello.NewsFeed.Cross.Dapr/"]
COPY ["./code/MattCanello.NewsFeed.RssReader/MattCanello.NewsFeed.RssReader.csproj", "MattCanello.NewsFeed.RssReader/"]
RUN dotnet restore "./MattCanello.NewsFeed.RssReader/./MattCanello.NewsFeed.RssReader.csproj"
COPY "./code" .
WORKDIR "/src/MattCanello.NewsFeed.RssReader"
RUN dotnet build "./MattCanello.NewsFeed.RssReader.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MattCanello.NewsFeed.RssReader.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MattCanello.NewsFeed.RssReader.dll"]