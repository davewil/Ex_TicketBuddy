FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Events/Host.Migrations/Host.Migrations.csproj", "Events/Host.Migrations/"]
COPY ["Events/Migrations/Migrations.csproj", "Events/Migrations/"]
RUN dotnet restore "Events/Host.Migrations/Host.Migrations.csproj"
COPY . .
WORKDIR "/src/Events/Host.Migrations"
RUN dotnet build "Host.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Host.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Host.Migrations.dll"]
