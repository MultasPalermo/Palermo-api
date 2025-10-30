# ===========================================================
# 🚀 Dockerfile corregido para estructura: Palermo-api/taller/Web
# ===========================================================

# Fase base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Fase build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar proyectos con rutas correctas (desde raíz)
COPY taller/Web/Web.csproj Web/
COPY taller/Business/Business.csproj Business/
COPY taller/Entity/Entity.csproj Entity/
COPY taller/Data/Data.csproj Data/
COPY taller/Helpers/Helpers.csproj Helpers/
COPY taller/Utilities/Utilities.csproj Utilities/
COPY taller/taller.sln .

# Restaurar dependencias
RUN dotnet restore "Web/Web.csproj"

# Copiar todo el código fuente
COPY taller/. .

# Compilar
WORKDIR /src/Web
RUN dotnet build "Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "Web.csproj" -c "${BUILD_CONFIGURATION:-Release}" -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]
