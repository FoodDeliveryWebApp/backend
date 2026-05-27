FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy project files first for layer-cached restore
COPY Explorer.sln .
COPY Explorer.API/Explorer.API.csproj Explorer.API/
COPY BuildingBlocks/Explorer.BuildingBlocks.Core/Explorer.BuildingBlocks.Core.csproj BuildingBlocks/Explorer.BuildingBlocks.Core/
COPY BuildingBlocks/Explorer.BuildingBlocks.Infrastructure/Explorer.BuildingBlocks.Infrastructure.csproj BuildingBlocks/Explorer.BuildingBlocks.Infrastructure/
COPY Modules/Stakeholders/Explorer.Stakeholders.API/Explorer.Stakeholders.API.csproj Modules/Stakeholders/Explorer.Stakeholders.API/
COPY Modules/Stakeholders/Explorer.Stakeholders.Core/Explorer.Stakeholders.Core.csproj Modules/Stakeholders/Explorer.Stakeholders.Core/
COPY Modules/Stakeholders/Explorer.Stakeholders.Infrastructure/Explorer.Stakeholders.Infrastructure.csproj Modules/Stakeholders/Explorer.Stakeholders.Infrastructure/

RUN dotnet restore Explorer.API/Explorer.API.csproj

COPY . .

RUN dotnet publish Explorer.API/Explorer.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "Explorer.API.dll"]
