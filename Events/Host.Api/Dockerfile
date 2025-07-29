FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Events/Host.Api/Host.Api.csproj", "Host.Api/"]
COPY ["Events/Host.Migrations/Host.Migrations.csproj", "Host.Migrations/"]
COPY ["Events/Migrations/Migrations.csproj", "Migrations/"]
COPY ["Events/Application/Application.csproj", "Application/"]
COPY ["Events/Domain/Domain.csproj", "Domain/"]
COPY ["Events/Domain.Messaging/Domain.Messaging.csproj", "Domain.Messaging/"]
COPY ["Events/Integration.Messaging/Integration.Messaging.csproj", "Integration.Messaging/"]
COPY ["Events/Persistence/Persistence.csproj", "Persistence/"]
RUN dotnet restore "./Host.Api/Host.Api.csproj"
COPY . .
WORKDIR "/src/Host.Api"
RUN dotnet build "./Host.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Host.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Host.Api.dll"]