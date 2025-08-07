FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Host.Dataseeder/Host.Dataseeder.csproj", "Host.Dataseeder/"]
COPY ["Application.Events/Application.Events.csproj", "Application.Events/"]
COPY ["Application.Users/Application.Users.csproj", "Application.Users/"]
COPY ["Domain.Events/Domain.Events.csproj", "Domain.Events/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Domain.Users/Domain.Users.csproj", "Domain.Users/"]
COPY ["Persistence.Events/Persistence.Events.csproj", "Persistence.Events/"]
COPY ["Persistence.Users/Persistence.Users.csproj", "Persistence.Users/"]
COPY ["Controllers.Events/Controllers.Events.csproj", "Controllers.Events/"]
COPY ["Controllers.Users/Controllers.Users.csproj", "Controllers.Users/"]
RUN dotnet restore "Host.Dataseeder/Host.Dataseeder.csproj"
COPY . .
WORKDIR "/src/Host.Dataseeder"
RUN dotnet build "Host.Dataseeder.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Host.Dataseeder"
RUN dotnet publish "Host.Dataseeder.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Host.Dataseeder.dll"]