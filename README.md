# Микросервисное приложение обработки заказов

Проект представляет собой учебную реализацию системы из трёх микросервисов для создания заказов, обработки платежей и отправки уведомлений.  
Разработан на **.NET 8** с использованием современных инструментов: Docker, Kafka, SignalR, MediatR, EF Core, Refit и др.

## Архитектура

- **OrderService** – входная точка. Создаёт заказ, резервирует оплату через PaymentService (HTTP/Rest), опционально отправляет событие в Kafka.
- **PaymentService** – управляет платежами. После успешной оплаты публикует событие в Kafka.
- **NotificationService** – подписывается на Kafka, отправляет пользователю уведомления через **SignalR** (WebSockets). Не имеет собственной БД.

## Запуск (Docker)

1. Убедитесь, что установлены **Docker** и **docker-compose**.
2. Клонируйте репозиторий:
   ```bash
   git clone https://github.com/xzefsx/SIMPL_Test.git
   cd Test_Simpl
   ```

3. Запустите все сервисы:
   ```bash
   docker-compose up --build
   ```

4. После запуска будут доступны:
- OrderService API → http://localhost:5001
- PaymentService API → http://localhost:5002
- SignalR хаб уведомлений → http://localhost:5003/notificationHub
- Kafka UI → http://localhost:8080 (топик payment-events)
- PostgreSQL Order → http://localhost:5431 (база order_db, пользователь postgres, пароль order_secure_pass)
- PostgreSQL Payment → http://localhost:5432 (база payment_db, пользователь postgres, пароль payment_secure_pass)

Конфигурация подключения к БД и Kafka задаётся через переменные окружения в docker-compose.yml. При старте автоматически создаётся топик payment-events (сервис kafka-init).

5. Также при работе через WSL может потребоваться запуск локального сервера:
   ```bash
   python3 -m http.server 8080
   ```

## API эндпоинты

**OrderService (/api/orders)**

| Метод | Эндпоинт | Описание |
| :--- | :--- | :--- |
| POST | /create | Создать заказ. |
| GET | /{order_id} | Получить информацию о заказе. |
| DELETE | /{order_id} | Удалить заказ. |

Тело **POST /create**:
   ```bash
   {
      "productId": 123,
      "amount": 2,
      "emailClient": "user@example.com",
      "price": 99.99,
      "phoneNumber": "+79991234567"
   }
   ```


**PaymentService (/api/payments)**
| Метод | Эндпоинт | Описание |
| :--- | :--- | :--- |
| POST | /create | Создать платёж для заказа. |
| PUT | /updateStatus/{paymentId}/{statusId} | Обновить статус платежа. |
| DELETE | /get/{paymentId} | Получить информацию о платеже. |

**NotificationService**
API отсутствует. Подключается к SignalR хабу:

```bash
   http://localhost:5003/notificationHub
   ```
В Postman (или другом клиенте) после подключения можно получать уведомления о событиях (например, об успешной оплате).

В проекте прописан скрипт ***full_test.sh***, в котором можно протестировать подключение к БД и отправку запросов. Также через страницу ***subscription.htm*** можно протестировать систему подписки на изменения записей, работающею через почту.

## Структура проекта (монорепозиторий)
```bash
├── OrderService/
│   ├── OrderService.WebApi/
│   └── OrderService.DataAccess.Postgres/
├── PaymentService/
│   ├── PaymentService.WebApi/
│   └── PaymentService.DataAccess.Postgres/
├── NotificationService/
│   └── NotificationService.WebApi/
├── docker-compose.yml
├── .gitignore
└── README.md
```

## Примечания
- Статус платежа: 0 – создан, 1 – успешен, 2 – отменён.
- При успешной оплате PaymentService отправляет событие в Kafka (payment-events).
- NotificationService читает события и рассылает их через SignalR всем подключённым клиентам.
- В docker-compose используется одна общая сеть mygeneral-network, все сервисы общаются по имени контейнера.
- OrderService обращается к PaymentService по внутреннему адресу http://payment-service:8080.
- Для сохранения данных БД используются Docker-тома order_db_data и payment_db_data.
