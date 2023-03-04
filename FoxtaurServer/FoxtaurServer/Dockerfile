FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FoxtaurServer/FoxtaurServer.csproj", "FoxtaurServer/"]
RUN dotnet restore "FoxtaurServer/FoxtaurServer.csproj"
COPY . .
WORKDIR "/src/FoxtaurServer"
RUN dotnet build "FoxtaurServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoxtaurServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoxtaurServer.dll"]
