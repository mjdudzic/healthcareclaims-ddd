FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["CoreDomain/ClaimsSubmission/ClaimsSubmission.Api/ClaimsSubmission.Api.csproj", "CoreDomain/ClaimsSubmission/ClaimsSubmission.Api/"]
COPY ["CoreDomain/ClaimsSubmission/ClaimsSubmission.Infrastructure/ClaimsSubmission.Infrastructure.csproj", "CoreDomain/ClaimsSubmission/ClaimsSubmission.Infrastructure/"]
COPY ["CoreDomain/ClaimsSubmission/ClaimsSubmission.Domain/ClaimsSubmission.Domain.csproj", "CoreDomain/ClaimsSubmission/ClaimsSubmission.Domain/"]
RUN dotnet restore "CoreDomain/ClaimsSubmission/ClaimsSubmission.Api/ClaimsSubmission.Api.csproj"
COPY . .
WORKDIR "/src/CoreDomain/ClaimsSubmission/ClaimsSubmission.Api"
RUN dotnet build "ClaimsSubmission.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ClaimsSubmission.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ClaimsSubmission.Api.dll"]
