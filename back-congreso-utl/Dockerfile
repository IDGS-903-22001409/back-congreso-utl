# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos el archivo .csproj y restauramos dependencias
COPY ["back-congreso-utl.csproj", "./"]
RUN dotnet restore "./back-congreso-utl.csproj"

# Copiamos todo el código y compilamos en modo Release
COPY . .
RUN dotnet publish "./back-congreso-utl.csproj" -c Release -o /app/publish

# Etapa final: imagen ligera para producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "back-congreso-utl.dll"]
