#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base 
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Microservice.Customer.Address.Api/Microservice.Customer.Address.Api.csproj", "Microservice.Customer.Address.Api/"]
RUN dotnet restore "./Microservice.Customer.Address.Api/./Microservice.Customer.Address.Api.csproj"
COPY . .
WORKDIR "/src/Microservice.Customer.Address.Api"
RUN dotnet build "./Microservice.Customer.Address.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Microservice.Customer.Address.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservice.Customer.Address.Api.dll"]