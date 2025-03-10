# Base com o runtime do .NET
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Fase de build com o SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Instala o dotnet-ef globalmente
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY ["src/OficinaTech.API/OficinaTech.API.csproj", "src/OficinaTech.API/"]
COPY ["src/OficinaTech.Application/OficinaTech.Application.csproj", "src/OficinaTech.Application/"]
COPY ["src/OficinaTech.Domain/OficinaTech.Domain.csproj", "src/OficinaTech.Domain/"]
COPY ["src/OficinaTech.Infrastructure/OficinaTech.Infrastructure.csproj", "src/OficinaTech.Infrastructure/"]
RUN dotnet restore "./src/OficinaTech.API/OficinaTech.API.csproj"

COPY . .
WORKDIR "/src/src/OficinaTech.API"
RUN dotnet build "./OficinaTech.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Fase de publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OficinaTech.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Fase final (agora com o SDK para suportar `dotnet-ef`)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 🔥 Adicionando `dotnet-ef` na fase final
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Executa a API
ENTRYPOINT ["dotnet", "OficinaTech.API.dll"]
