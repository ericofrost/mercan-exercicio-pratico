FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ER.WebApi/ER.WebApi.csproj", "ER.WebApi/"]
COPY ["ER.Application/ER.Application.csproj", "ER.Application/"]
COPY ["ER.Infrastructure/ER.Infrastructure.csproj", "ER.Infrastructure/"]
COPY ["ER.Domain/ER.Domain.csproj", "ER.Domain/"]
RUN dotnet restore "ER.WebApi/ER.WebApi.csproj"
COPY . .
WORKDIR "/src/ER.WebApi"
RUN dotnet build "./ER.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ER.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
USER root
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
COPY --from=publish /app/publish .
HEALTHCHECK --interval=30s --timeout=5s --start-period=15s --retries=3 \
  CMD curl --fail http://localhost:8080/healthz || exit 1
ENTRYPOINT ["dotnet", "ER.WebApi.dll"]
