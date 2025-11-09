# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos el archivo .csproj desde la carpeta del proyecto
COPY back-congreso-utl/back-congreso-utl.csproj back-congreso-utl/
RUN dotnet restore "back-congreso-utl/back-congreso-utl.csproj"

# Copiamos todo el código y compilamos
COPY . .
WORKDIR /src/back-congreso-utl
RUN dotnet publish "back-congreso-utl.csproj" -c Release -o /app/publish

# Etapa final: imagen ligera para producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "back-congreso-utl.dll"]
