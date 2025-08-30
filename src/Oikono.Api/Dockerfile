FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /
COPY ["src/Oikono.Api/Oikono.Api.csproj", "src/Oikono.Api/"]
COPY ["src/Oikono.Domain/Oikono.Domain.csproj", "src/Oikono.Domain/"]
COPY ["src/Oikono.Application/Oikono.Application.csproj", "src/Oikono.Application/"]
COPY ["src/Oikono.Infrastructure/Oikono.Infrastructure.csproj", "src/Oikono.Infrastructure/"]
COPY ["src/SourceGenerators/SourceGenerators/SourceGenerators.csproj", "src/SourceGenerators/SourceGenerators/"]
COPY ["src/Oikono.Infrastructure/Oikono.Infrastructure.csproj", "src/Oikono.Infrastructure/"]
RUN dotnet restore "src/Oikono.Api/Oikono.Api.csproj"
COPY . .
WORKDIR "/src/Oikono.Api"
RUN dotnet build "Oikono.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Oikono.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Oikono.Api.dll"]
