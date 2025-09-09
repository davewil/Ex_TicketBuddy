FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Domain.Events/Domain.Events.csproj", "Domain.Events/"]
COPY ["Domain.Tickets/Domain.Tickets.csproj", "Domain.Tickets/"]
COPY ["Domain.Users/Domain.Users.csproj", "Domain.Users/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application.Events/Application.Events.csproj", "Application.Events/"]
COPY ["Application.Tickets/Application.Tickets.csproj", "Application.Tickets/"]
COPY ["Application.Users/Application.Users.csproj", "Application.Users/"]
COPY ["Controllers.Events/Controllers.Events.csproj", "Controllers.Events/"]
COPY ["Controllers.Tickets/Controllers.Tickets.csproj", "Controllers.Tickets/"]
COPY ["Controllers.Users/Controllers.Users.csproj", "Controllers.Users/"]
COPY ["Integration.Events.Messaging/Integration.Events.Messaging.csproj", "Integration.Events.Messaging/"]
COPY ["Integration.Users.Messaging/Integration.Users.Messaging.csproj", "Integration.Users.Messaging/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Infrastructure.Events/Infrastructure.Events.csproj", "Infrastructure.Events/"]
COPY ["Infrastructure.Users/Infrastructure.Users.csproj", "Infrastructure.Users/"]
COPY ["Infrastructure.Tickets/Infrastructure.Tickets.csproj", "Infrastructure.Tickets/"]
COPY ["Host/Host.csproj", "Host/"]
RUN dotnet restore "Host/Host.csproj"
COPY . .
WORKDIR "/src/Host"
RUN dotnet build "Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Host"
RUN dotnet publish "Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Host.dll"]