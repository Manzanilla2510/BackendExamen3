# ================= BUILD =================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiamos solo el csproj primero
COPY ["Examen3.csproj", "./"]

# Restore
RUN dotnet restore "Examen3.csproj"

# Copiamos todo el código
COPY . .

# Compilamos
RUN dotnet build "Examen3.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ================= PUBLISH =================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Examen3.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ================= FINAL =================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiamos la publicación
COPY --from=publish /app/publish ./

EXPOSE 8080

ENTRYPOINT ["dotnet", "Examen3.dll"]
