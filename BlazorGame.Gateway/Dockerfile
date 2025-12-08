# BlazorGame.Gateway/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["BlazorGame.Gateway/BlazorGame.Gateway.csproj", "BlazorGame.Gateway/"]
RUN dotnet restore "BlazorGame.Gateway/BlazorGame.Gateway.csproj"
COPY . .
WORKDIR "/src/BlazorGame.Gateway"
RUN dotnet build "BlazorGame.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorGame.Gateway.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "BlazorGame.Gateway.dll"]