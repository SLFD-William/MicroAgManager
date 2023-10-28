#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MicroAgManager/Host/Host.csproj", "MicroAgManager/Host/"]
COPY ["MicroAgManager/Persistence/BackEnd.Persistence.csproj", "MicroAgManager/Persistence/"]
COPY ["MicroAgManager/Domain/Domain.csproj", "MicroAgManager/Domain/"]
COPY ["MicroAgManager/Server/BackEnd.csproj", "MicroAgManager/Server/"]
COPY ["MicroAgManager/WASM/WASM.csproj", "MicroAgManager/WASM/"]
COPY ["MicroAgManager/Client/FrontEnd.csproj", "MicroAgManager/Client/"]
COPY ["MicroAgManager/Client.Persistence/FrontEnd.Persistence.csproj", "MicroAgManager/Client.Persistence/"]
RUN dotnet restore "MicroAgManager/Host/Host.csproj"
COPY . .
WORKDIR "/src/MicroAgManager/Host"
RUN dotnet build "Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Host.dll"]