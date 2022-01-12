FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["IAWebAPI/IAWebAPI.csproj", "IAWebAPI/"]
RUN dotnet restore "IAWebAPI/IAWebAPI.csproj"
COPY . .
WORKDIR "/src/IAWebAPI"
RUN dotnet build "IAWebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IAWebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IAWebAPI.dll"]
