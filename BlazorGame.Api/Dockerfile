# BlazorGame.Api/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["BlazorGame.Api/BlazorGame.Api.csproj", "BlazorGame.Api/"]
COPY ["BlazorGame.Domain/BlazorGame.Domain.csproj", "BlazorGame.Domain/"]
RUN dotnet restore "BlazorGame.Api/BlazorGame.Api.csproj"
COPY . .
WORKDIR "/src/BlazorGame.Api"
RUN dotnet build "BlazorGame.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorGame.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "BlazorGame.Api.dll"]