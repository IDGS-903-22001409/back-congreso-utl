# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiamos el archivo de proyecto y restauramos dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiamos todo el código y compilamos en modo Release
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Configuramos la variable de entorno para que escuche el puerto dinámico
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT

# Comando de inicio
ENTRYPOINT ["dotnet", "back-congreso-utl.dll"]
