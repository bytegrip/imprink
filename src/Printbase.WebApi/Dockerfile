FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Printbase.WebApi/Printbase.WebApi.csproj", "src/Printbase.WebApi/"]
RUN dotnet restore "src/Printbase.WebApi/Printbase.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Printbase.WebApi"
RUN dotnet build "Printbase.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Printbase.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Printbase.WebApi.dll"]
