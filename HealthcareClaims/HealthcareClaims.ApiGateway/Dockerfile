FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["HealthcareClaims.ApiGateway/HealthcareClaims.ApiGateway.csproj", "HealthcareClaims.ApiGateway/"]
RUN dotnet restore "HealthcareClaims.ApiGateway/HealthcareClaims.ApiGateway.csproj"
COPY . .
WORKDIR "/src/HealthcareClaims.ApiGateway"
RUN dotnet build "HealthcareClaims.ApiGateway.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HealthcareClaims.ApiGateway.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HealthcareClaims.ApiGateway.dll"]