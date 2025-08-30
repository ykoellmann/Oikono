FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /
COPY ["src/Oikono.LoadBalancer/Oikono.LoadBalancer.csproj", "src/Oikono.LoadBalancer/"]
RUN dotnet restore "src/Oikono.LoadBalancer/Oikono.LoadBalancer.csproj"
COPY . .
WORKDIR "/src/Oikono.LoadBalancer"
RUN dotnet build "Oikono.LoadBalancer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Oikono.LoadBalancer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Oikono.LoadBalancer.dll"]
