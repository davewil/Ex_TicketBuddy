FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tickets/Host.Migrations/Host.Migrations.csproj", "Tickets/Host.Migrations/"]
COPY ["Tickets/Migrations/Migrations.csproj", "Tickets/Migrations/"]
RUN dotnet restore "Tickets/Host.Migrations/Host.Migrations.csproj"
COPY . .
WORKDIR "/src/Tickets/Host.Migrations"
RUN dotnet build "Host.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Host.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Host.Migrations.dll"]
