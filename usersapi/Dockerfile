FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY ["usersapi.csproj", "."]
RUN dotnet restore

COPY . .
RUN dotnet publish "usersapi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "usersapi.dll"]