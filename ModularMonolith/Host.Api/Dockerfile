FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Host.Api/Host.Api.csproj", "Host.Api/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Domain.Messaging/Domain.Messaging.csproj", "Domain.Messaging/"]
COPY ["Integration.Messaging/Integration.Messaging.csproj", "Integration.Messaging/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
RUN dotnet restore "Host.Api/Host.Api.csproj"
COPY . .
WORKDIR "/src/Host.Api"
RUN dotnet build "Host.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Host.Api"
RUN dotnet publish "Host.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Host.Api.dll"]