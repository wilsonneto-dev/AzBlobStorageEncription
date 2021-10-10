#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/BlobLab.Backend/BlobLab.Backend.csproj", "src/BlobLab.Backend/"]
RUN dotnet restore "src/BlobLab.Backend/BlobLab.Backend.csproj"
COPY . .
WORKDIR "/src/src/BlobLab.Backend"
RUN dotnet build "BlobLab.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlobLab.Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlobLab.Backend.dll"]