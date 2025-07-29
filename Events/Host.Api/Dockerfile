FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Events/Host.Api/Host.Api.csproj", "Events/Host.Api/"]
COPY ["Events/Application/Application.csproj", "Events/Application/"]
COPY ["Events/Domain/Domain.csproj", "Events/Domain/"]
COPY ["Events/Domain.Messaging/Domain.Messaging.csproj", "Events/Domain.Messaging/"]
COPY ["Events/Integration.Messaging/Integration.Messaging.csproj", "Events/Integration.Messaging/"]
COPY ["Events/Persistence/Persistence.csproj", "Events/Persistence/"]
RUN dotnet restore "Events/Host.Api/Host.Api.csproj"
COPY . .
WORKDIR "/src/Events/Host.Api"
RUN dotnet build "Host.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Events/Host.Api"
RUN dotnet publish "Host.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Host.Api.dll"]