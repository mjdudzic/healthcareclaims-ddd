FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Subdomains/InsuredMembers/InsuredMembers.Api/InsuredMembers.Api.csproj", "Subdomains/InsuredMembers/InsuredMembers.Api/"]
RUN dotnet restore "Subdomains/InsuredMembers/InsuredMembers.Api/InsuredMembers.Api.csproj"
COPY . .
WORKDIR "/src/Subdomains/InsuredMembers/InsuredMembers.Api"
RUN dotnet build "InsuredMembers.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "InsuredMembers.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "InsuredMembers.Api.dll"]