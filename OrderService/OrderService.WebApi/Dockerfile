FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj файлы и восстанавливаем зависимости
COPY ["OrderService/OrderService.WebApi/OrderServiceWebApp.csproj", "OrderService/OrderService.WebApi/"]
COPY ["OrderService/OrderService.DataAccess.Postgres/OrderServiceDB.csproj", "OrderService/OrderService.DataAccess.Postgres/"]
RUN dotnet restore "OrderService/OrderService.WebApi/OrderServiceWebApp.csproj"

# Копируем весь исходный код
COPY . .

# Собираем проект
WORKDIR "/src/OrderService/OrderService.WebApi"
RUN dotnet build "OrderServiceWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderServiceWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderServiceWebApp.dll"]