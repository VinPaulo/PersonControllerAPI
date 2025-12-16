# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Person.csproj", "."]
RUN dotnet restore "./Person.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Person.csproj" -c Release -o /app/build
RUN dotnet publish "Person.csproj" -c Release -o /app/publish

# Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Person.dll"]
