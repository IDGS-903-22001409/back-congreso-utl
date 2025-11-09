# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar todos los archivos del proyecto
COPY . ./

# Restaurar dependencias
RUN dotnet restore back-congreso-utl.csproj

# Compilar en modo Release
RUN dotnet publish back-congreso-utl.csproj -c Release -o out

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Configurar entorno y puerto
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT

ENTRYPOINT ["dotnet", "back-congreso-utl.dll"]
