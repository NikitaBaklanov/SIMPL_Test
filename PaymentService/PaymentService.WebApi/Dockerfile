FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj файлы
COPY ["PaymentService/PaymentService.WebApi/PaymentServiceWebApp.csproj", "PaymentService/PaymentService.WebApi/"]
COPY ["PaymentService/PaymentService.DataAccess.Postgres/PaymentServiceDB.csproj", "PaymentService/PaymentService.DataAccess.Postgres/"]
RUN dotnet restore "PaymentService/PaymentService.WebApi/PaymentServiceWebApp.csproj"

# Копируем весь код
COPY . .

WORKDIR "/src/PaymentService/PaymentService.WebApi"
RUN dotnet build "PaymentServiceWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentServiceWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentServiceWebApp.dll"]