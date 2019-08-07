FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["CoreDomain/ClaimsVetting/ClaimsVetting.Api/ClaimsVetting.Api.csproj", "CoreDomain/ClaimsVetting/ClaimsVetting.Api/"]
RUN dotnet restore "CoreDomain/ClaimsVetting/ClaimsVetting.Api/ClaimsVetting.Api.csproj"
COPY . .
WORKDIR "/src/CoreDomain/ClaimsVetting/ClaimsVetting.Api"
RUN dotnet build "ClaimsVetting.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ClaimsVetting.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ClaimsVetting.Api.dll"]