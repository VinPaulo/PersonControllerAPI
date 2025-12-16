# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Client/Client.csproj", "Client/"]
RUN dotnet restore "Client/Client.csproj"
COPY . .
WORKDIR "/src/Client"
RUN dotnet publish "Client.csproj" -c Release -o /app/publish

# Run Stage
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY Client/nginx.conf /etc/nginx/conf.d/default.conf

# Expose port
EXPOSE 80

# Replace API URL and start Nginx
CMD ["/bin/sh", "-c", "if [ ! -z \"$API_URL\" ]; then sed -i \"s|http://localhost:5028|$API_URL|g\" appsettings.json; fi && nginx -g 'daemon off;'"]
