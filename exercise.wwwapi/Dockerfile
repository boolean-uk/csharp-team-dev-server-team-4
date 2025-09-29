# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
ARG BUILD_CONFIGURATION=Release
COPY ["exercise.wwwapi.csproj", "./"]
RUN dotnet restore "./exercise.wwwapi.csproj"
COPY . .
RUN dotnet publish "./exercise.wwwapi.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_EnableDiagnostics=0
EXPOSE 8080
COPY --from=build --chown=app:app /app/publish .
USER app
ENTRYPOINT ["dotnet", "exercise.wwwapi.dll"]

