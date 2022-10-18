FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WPPostFromVideoConsole/WPPostFromVideoConsole.csproj", "WPPostFromVideoConsole/"]
RUN dotnet restore "WPPostFromVideoConsole/WPPostFromVideoConsole.csproj"
COPY . .
WORKDIR "/src/WPPostFromVideoConsole"
RUN dotnet build "WPPostFromVideoConsole.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WPPostFromVideoConsole.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WPPostFromVideoConsole.dll"]
