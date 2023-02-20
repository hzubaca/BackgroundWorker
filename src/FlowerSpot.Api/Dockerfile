#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/FlowerSpot.Api/FlowerSpot.Api.csproj", "src/FlowerSpot.Api/"]
COPY ["src/FlowerSpot.Infrastructure/FlowerSpot.Infrastructure.csproj", "src/FlowerSpot.Infrastructure/"]
COPY ["src/FlowerSpot.Application/FlowerSpot.Application.csproj", "src/FlowerSpot.Application/"]
COPY ["src/FlowerSpot.Domain/FlowerSpot.Domain.csproj", "src/FlowerSpot.Domain/"]
COPY ["src/shared/FlowerSpot.SharedKernel/FlowerSpot.SharedKernel.csproj", "src/shared/FlowerSpot.SharedKernel/"]
RUN dotnet restore "src/FlowerSpot.Api/FlowerSpot.Api.csproj"
COPY . .
WORKDIR "/src/src/FlowerSpot.Api"
RUN dotnet build "FlowerSpot.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlowerSpot.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlowerSpot.Api.dll"]