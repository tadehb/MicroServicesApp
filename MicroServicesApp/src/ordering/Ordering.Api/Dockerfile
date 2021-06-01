#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/ordering/Ordering.Api/Ordering.Api.csproj", "src/ordering/Ordering.Api/"]
COPY ["src/ordering/Ordering.Application/Ordering.Application.csproj", "src/ordering/Ordering.Application/"]
COPY ["src/ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "src/ordering/Ordering.Infrastructure/"]
COPY ["src/ordering/Ordering.Core/Ordering.Core.csproj", "src/ordering/Ordering.Core/"]
COPY ["src/Common/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/Common/EventBusRabbitMQ/"]
RUN dotnet restore "src/ordering/Ordering.Api/Ordering.Api.csproj"
COPY . .
WORKDIR "/src/src/ordering/Ordering.Api"
RUN dotnet build "Ordering.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.Api.dll"]
