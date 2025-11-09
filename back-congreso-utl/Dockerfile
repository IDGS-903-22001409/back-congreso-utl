# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY back-congreso-utl/back-congreso-utl.csproj ./back-congreso-utl/
RUN dotnet restore "back-congreso-utl/back-congreso-utl.csproj"

COPY . .
WORKDIR /src/back-congreso-utl
RUN dotnet publish -c Release -o /app/publish

# Etapa final: imagen ligera
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "back-congreso-utl.dll"]
