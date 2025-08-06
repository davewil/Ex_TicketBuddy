FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Users/Host.Api/Host.Api.csproj", "Users/Host.Api/"]
COPY ["Users/Application/Application.csproj", "Users/Application/"]
COPY ["Users/Domain/Domain.csproj", "Users/Domain/"]
COPY ["Users/Domain.Messaging/Domain.Messaging.csproj", "Users/Domain.Messaging/"]
COPY ["Users/Integration.Messaging/Integration.Messaging.csproj", "Users/Integration.Messaging/"]
COPY ["Users/Persistence/Persistence.csproj", "Users/Persistence/"]
RUN dotnet restore "Users/Host.Api/Host.Api.csproj"
COPY . .
WORKDIR "/src/Users/Host.Api"
RUN dotnet build "Host.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Users/Host.Api"
RUN dotnet publish "Host.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Host.Api.dll"]