# ============================================================================
# Portfolio.Api — multi-stage build
# Build context must be the Portfolio/ folder (contains src/ and the .sln):
#   docker build -f src/Portfolio.Api/Dockerfile -t portfolio-api .
# ============================================================================

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Restore first for layer caching
COPY src/Portfolio.Domain/Portfolio.Domain.csproj              src/Portfolio.Domain/
COPY src/Portfolio.Application/Portfolio.Application.csproj    src/Portfolio.Application/
COPY src/Portfolio.Persistence/Portfolio.Persistence.csproj    src/Portfolio.Persistence/
COPY src/Portfolio.Infrastructure/Portfolio.Infrastructure.csproj src/Portfolio.Infrastructure/
COPY src/Portfolio.Api/Portfolio.Api.csproj                    src/Portfolio.Api/
RUN dotnet restore src/Portfolio.Api/Portfolio.Api.csproj

COPY src/ src/
RUN dotnet publish src/Portfolio.Api/Portfolio.Api.csproj -c Release -o /app/publish -p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Public documents are mounted, never baked into the image:
#   -v /host/public-docs:/app/public-documents
# and set Documents__PhysicalRoot=/app/public-documents
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Portfolio.Api.dll"]
