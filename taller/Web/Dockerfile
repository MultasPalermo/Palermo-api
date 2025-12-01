# Etapa base (CON PLAYWRIGHT — ESTA ES LA BUENA)
FROM mcr.microsoft.com/playwright/dotnet:v1.54.0-jammy AS final-base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Web/Web.csproj Web/
COPY Business/Business.csproj Business/
COPY Entity/Entity.csproj Entity/
COPY Data/Data.csproj Data/
COPY Helpers/Helpers.csproj Helpers/
COPY Utilities/Utilities.csproj Utilities/
COPY taller.sln .

RUN dotnet restore "Web/Web.csproj"

COPY . .
WORKDIR /src/Web
RUN dotnet build "Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagen final (runtime + Playwright + Chromium)
FROM final-base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]

