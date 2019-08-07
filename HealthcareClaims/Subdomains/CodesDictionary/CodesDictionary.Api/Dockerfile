FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Subdomains/CodesDictionary/CodesDictionary.Api/CodesDictionary.Api.csproj", "Subdomains/CodesDictionary/CodesDictionary.Api/"]
RUN dotnet restore "Subdomains/CodesDictionary/CodesDictionary.Api/CodesDictionary.Api.csproj"
COPY . .
WORKDIR "/src/Subdomains/CodesDictionary/CodesDictionary.Api"
RUN dotnet build "CodesDictionary.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CodesDictionary.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CodesDictionary.Api.dll"]