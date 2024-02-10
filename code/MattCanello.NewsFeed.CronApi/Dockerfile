#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./code/MattCanello.NewsFeed.CronApi/MattCanello.NewsFeed.CronApi.csproj", "MattCanello.NewsFeed.CronApi/"]
COPY ["./code/MattCanello.NewsFeed.Cross.CloudEvents/MattCanello.NewsFeed.Cross.CloudEvents.csproj", "MattCanello.NewsFeed.Cross.CloudEvents/"]
COPY ["./code/MattCanello.NewsFeed.Cross.Abstractions/MattCanello.NewsFeed.Cross.Abstractions.csproj", "MattCanello.NewsFeed.Cross.Abstractions/"]
RUN dotnet restore "./MattCanello.NewsFeed.CronApi/./MattCanello.NewsFeed.CronApi.csproj"
COPY . .
WORKDIR "/src/MattCanello.NewsFeed.CronApi"
RUN dotnet build "./MattCanello.NewsFeed.CronApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MattCanello.NewsFeed.CronApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MattCanello.NewsFeed.CronApi.dll"]