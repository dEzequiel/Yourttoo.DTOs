# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Yourttoo.Api/Yourttoo.Api.csproj Yourttoo.Api/
COPY Yourttoo.DTOs/Yourttoo.DTOs.csproj Yourttoo.DTOs/
RUN dotnet restore Yourttoo.Api/Yourttoo.Api.csproj
COPY . .
WORKDIR /src/Yourttoo.Api
RUN dotnet publish Yourttoo.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
COPY Yourttoo.Api/dev.db /app/dev.db

# 🔸 Sólo el puerto que Render asigna
ENV ASPNETCORE_HTTP_PORTS=${PORT}
# (alternativa equivalente)
# ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

ENV ASPNETCORE_ENVIRONMENT=Production

# No expongas puertos fijos en Render
# EXPOSE 8080
# EXPOSE 8081

ENTRYPOINT ["dotnet", "Yourttoo.Api.dll"]
