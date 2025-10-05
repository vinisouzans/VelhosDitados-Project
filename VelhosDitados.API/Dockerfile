# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivo de projeto e restaurar dependências
COPY ["VelhosDitados.API.csproj", "./"]
RUN dotnet restore "VelhosDitados.API.csproj"

# Copiar todo o código e compilar
COPY . .
WORKDIR "/src"
RUN dotnet build "VelhosDitados.API.csproj" -c Release -o /app/build

# Publicar a aplicação
FROM build AS publish
RUN dotnet publish "VelhosDitados.API.csproj" -c Release -o /app/publish

# Estágio final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VelhosDitados.API.dll"]