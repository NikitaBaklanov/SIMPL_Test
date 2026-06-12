FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj
COPY ["NotificationService/NotificationService.WebApi/NotificationServiceWebApp.csproj", "NotificationService/NotificationService.WebApi/"]
RUN dotnet restore "NotificationService/NotificationService.WebApi/NotificationServiceWebApp.csproj"

# Копируем весь код
COPY . .

WORKDIR "/src/NotificationService/NotificationService.WebApi"
RUN dotnet build "NotificationServiceWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationServiceWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationServiceWebApp.dll"]