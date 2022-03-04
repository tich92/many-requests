#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SiteAtt.Console/SiteAtt.Console.csproj", "SiteAtt.Console/"]
COPY ["Core/Core.csproj", "Core/"]
RUN dotnet restore "SiteAtt.Console/SiteAtt.Console.csproj"
COPY . .
WORKDIR "/src/SiteAtt.Console"
RUN dotnet build "SiteAtt.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SiteAtt.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SiteAtt.Console.dll"]