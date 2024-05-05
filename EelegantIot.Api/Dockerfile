FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EelegantIot.Api/EelegantIot.Api.csproj", "EelegantIot.Api/"]
RUN dotnet restore "EelegantIot.Api/EelegantIot.Api.csproj"
COPY . .
WORKDIR "/src/EelegantIot.Api"
RUN dotnet build "EelegantIot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EelegantIot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EelegantIot.Api.dll"]
