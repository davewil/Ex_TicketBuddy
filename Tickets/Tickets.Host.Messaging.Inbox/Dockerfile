FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tickets/Host.Messaging.Inbox/Host.Messaging.Inbox.csproj", "Tickets/Host.Messaging.Inbox/"]
COPY ["Tickets/Persistence/Persistence.csproj", "Tickets/Persistence/"]
COPY ["Integration.Messaging.Inbound/Integration.Messaging.Inbound.csproj", "Integration.Messaging.Inbound/"]
COPY ["Tickets/Domain/Domain.csproj", "Tickets/Domain/"]
RUN dotnet restore "Tickets/Host.Messaging.Inbox/Host.Messaging.Inbox.csproj"
COPY . .
WORKDIR "/src/Tickets/Host.Messaging.Inbox"
RUN dotnet build "Host.Messaging.Inbox.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Host.Messaging.Inbox.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Host.Messaging.Inbox.dll"]
