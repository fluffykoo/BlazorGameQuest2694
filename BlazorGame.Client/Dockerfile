# BlazorGame.Client/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["BlazorGame.Client/BlazorGame.Client.csproj", "BlazorGame.Client/"]
COPY ["BlazorGame.Domain/BlazorGame.Domain.csproj", "BlazorGame.Domain/"]
RUN dotnet restore "BlazorGame.Client/BlazorGame.Client.csproj"
COPY . .
WORKDIR "/src/BlazorGame.Client"
RUN dotnet build "BlazorGame.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorGame.Client.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY BlazorGame.Client/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80