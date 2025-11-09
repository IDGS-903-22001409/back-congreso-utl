# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos el archivo .csproj
COPY *.csproj ./
RUN dotnet restore

# Copiamos el resto del código y compilamos
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Etapa final: imagen ligera para producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "back-congreso-utl.dll"]
