#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Tr3Line.Assessment.Api.csproj", "."]
RUN dotnet restore "./Tr3Line.Assessment.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Tr3Line.Assessment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tr3Line.Assessment.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Tr3Line.Assessment.Api.dll
#ENTRYPOINT ["dotnet", "Tr3Line.Assessment.Api.dll"]