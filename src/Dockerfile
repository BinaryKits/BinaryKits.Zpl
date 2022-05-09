#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

#Install System.Drawing native dependencies, required for NetBarcode
RUN apt-get update \
&& apt-get install -y --allow-unauthenticated \
    libc6-dev \
    libgdiplus \
    libx11-dev \
 && rm -rf /var/lib/apt/lists/*

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BinaryKits.Zpl.Viewer.WebApi/BinaryKits.Zpl.Viewer.WebApi.csproj", "BinaryKits.Zpl.Viewer.WebApi/"]
COPY ["BinaryKits.Zpl.Viewer/BinaryKits.Zpl.Viewer.csproj", "BinaryKits.Zpl.Viewer/"]
COPY ["BinaryKits.Zpl.Label/BinaryKits.Zpl.Label.csproj", "BinaryKits.Zpl.Label/"]
RUN dotnet restore "BinaryKits.Zpl.Viewer.WebApi/BinaryKits.Zpl.Viewer.WebApi.csproj"
COPY . .
WORKDIR "/src/BinaryKits.Zpl.Viewer.WebApi"
RUN dotnet build "BinaryKits.Zpl.Viewer.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BinaryKits.Zpl.Viewer.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BinaryKits.Zpl.Viewer.WebApi.dll"]