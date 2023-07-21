FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src


COPY ["./ProductStore.Application/ProductStore.Application.csproj", "src/ProductStore.Application/"]
COPY ["./ProductStore.Contracts/ProductStore.Contracts.csproj", "src/ProductStore.Contracts/"]
COPY ["./ProductStore.Infrastructure/ProductStore.Infrastructure.csproj", "src/ProductStore.Infrastructure/"]
COPY ["./ProductStore.Domain/ProductStore.Domain.csproj", "src/ProductStore.Domain/"]
COPY ["./ProductStore.Api/ProductStore.Api.csproj", "src/ProductStore.Api/"]

RUN dotnet restore "src/ProductStore.Api/ProductStore.Api.csproj"


COPY . ./

WORKDIR "/src/ProductStore.Api/"
RUN dotnet build -c Release -o /app/build


FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet","ProductStore.Api.dll" ]

