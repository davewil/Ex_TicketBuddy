FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Events/Host.Messaging.Outbox/Host.Messaging.Outbox.csproj", "Events/Host.Messaging.Outbox/"]
COPY ["Events/Persistence/Persistence.csproj", "Events/Persistence/"]
COPY ["Events/Integration.Messaging/Integration.Messaging.csproj", "Events/Integration.Messaging/"]
COPY ["Events/Domain/Domain.csproj", "Events/Domain/"]
COPY ["Events/Domain.Messaging/Domain.Messaging.csproj", "Events/Domain.Messaging/"]
RUN dotnet restore "Events/Host.Messaging.Outbox/Host.Messaging.Outbox.csproj"
COPY . .
WORKDIR "/src/Events/Host.Messaging.Outbox"
RUN dotnet build "Host.Messaging.Outbox.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Host.Messaging.Outbox.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Host.Messaging.Outbox.dll"]
