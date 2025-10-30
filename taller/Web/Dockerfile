# Dockerfile corregido

# Fase base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Fase build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar los proyectos y la solución
COPY Web/Web.csproj Web/
COPY Business/Business.csproj Business/
COPY Entity/Entity.csproj Entity/
COPY Data/Data.csproj Data/
COPY Helpers/Helpers.csproj Helpers/
COPY Utilities/Utilities.csproj Utilities/
COPY taller.sln .

# Restaurar paquetes
RUN dotnet restore "./Web/Web.csproj"

# Copiar todo el código
COPY . .

# Construir el proyecto Web
WORKDIR "/src/Web"
RUN dotnet build "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]

