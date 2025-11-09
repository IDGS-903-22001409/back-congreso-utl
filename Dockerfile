# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiamos el archivo .csproj desde la carpeta del proyecto
COPY back-congreso-utl/*.csproj ./back-congreso-utl/
RUN dotnet restore back-congreso-utl/back-congreso-utl.csproj

# Copiamos todo el código y compilamos
COPY . ./
WORKDIR /app/back-congreso-utl
RUN dotnet publish -c Release -o out

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/back-congreso-utl/out .

ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT

ENTRYPOINT ["dotnet", "back-congreso-utl.dll"]
