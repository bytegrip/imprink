FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Imprink.WebApi/Imprink.WebApi.csproj", "src/Imprink.WebApi/"]
COPY ["src/Imprink.Application/Imprink.Application.csproj", "src/Imprink.Application/"]
COPY ["src/Imprink.Domain/Imprink.Domain.csproj", "src/Imprink.Domain/"]
COPY ["src/Imprink.Infrastructure/Imprink.Infrastructure.csproj", "src/Imprink.Infrastructure/"]
RUN dotnet restore "src/Imprink.WebApi/Imprink.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Imprink.WebApi"
RUN dotnet build "Imprink.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Imprink.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Imprink.WebApi.dll"]