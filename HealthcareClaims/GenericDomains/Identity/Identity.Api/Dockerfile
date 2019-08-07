FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["GenericDomains/Identity/Identity.Api/Identity.Api.csproj", "GenericDomains/Identity/Identity.Api/"]
RUN dotnet restore "GenericDomains/Identity/Identity.Api/Identity.Api.csproj"
COPY . .
WORKDIR "/src/GenericDomains/Identity/Identity.Api"
RUN dotnet build "Identity.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Identity.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]