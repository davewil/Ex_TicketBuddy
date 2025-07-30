FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Users/Host.Messaging.Outbox/Host.Messaging.Outbox.csproj", "Users/Host.Messaging.Outbox/"]
COPY ["Users/Persistence/Persistence.csproj", "Users/Persistence/"]
COPY ["Users/Integration.Messaging/Integration.Messaging.csproj", "Users/Integration.Messaging/"]
COPY ["Users/Domain/Domain.csproj", "Users/Domain/"]
COPY ["Users/Domain.Messaging/Domain.Messaging.csproj", "Users/Domain.Messaging/"]
RUN dotnet restore "Users/Host.Messaging.Outbox/Host.Messaging.Outbox.csproj"
COPY . .
WORKDIR "/src/Users/Host.Messaging.Outbox"
RUN dotnet build "Host.Messaging.Outbox.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Host.Messaging.Outbox.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Host.Messaging.Outbox.dll"]
