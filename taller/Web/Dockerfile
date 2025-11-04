FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# ✅ Copiar proyectos con rutas relativas al contexto (sin "taller/")
COPY Web/Web.csproj Web/
COPY Business/Business.csproj Business/
COPY Entity/Entity.csproj Entity/
COPY Data/Data.csproj Data/
COPY Helpers/Helpers.csproj Helpers/
COPY Utilities/Utilities.csproj Utilities/
COPY taller.sln .

# Restaurar dependencias
RUN dotnet restore "Web/Web.csproj"

# Copiar todo el código fuente
COPY . .

WORKDIR /src/Web
RUN dotnet build "Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Web.csproj" -c "${BUILD_CONFIGURATION:-Release}" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]
