#!/bin/bash

# Цветной вывод
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${GREEN}=== Интеграционный тест микросервисов ===${NC}"

# Базовые URL
ORDER_URL="http://localhost:5001"
PAYMENT_URL="http://localhost:5002"
NOTIFICATION_URL="http://localhost:5003"

# 1. Проверка доступности сервисов
echo -e "\n${YELLOW}1. Проверка доступности сервисов...${NC}"

check_service() {
    local url=$1
    local name=$2
    curl -s -o /dev/null -w "%{http_code}" "$url" | grep -q "200\|404"
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}$name доступен${NC}"
    else
        echo -e "${RED}$name не отвечает${NC}"
        exit 1
    fi
}

check_service "$ORDER_URL/swagger/index.html" "OrderService"
check_service "$PAYMENT_URL/swagger/index.html" "PaymentService"
check_service "$NOTIFICATION_URL/health" "NotificationService"

# 2. Создание заказа
echo -e "\n${YELLOW}2. Создание заказа...${NC}"
ORDER_RESPONSE=$(curl -s -X POST "$ORDER_URL/api/orders/create" \
  -H "Content-Type: application/json" \
  -d '{
    "productId": 100500,
    "amount": 2,
    "emailClient": "test@example.com",
    "price": 99.99,
    "phoneNumber": "+79991234567"
  }')

ORDER_ID=$(echo "$ORDER_RESPONSE" | jq -r '.id')
if [ "$ORDER_ID" != "null" ] && [ -n "$ORDER_ID" ]; then
    echo -e "${GREEN}Заказ создан, ID: $ORDER_ID${NC}"
else
    echo -e "${RED}Ошибка создания заказа: $ORDER_RESPONSE${NC}"
    exit 1
fi

# 3. Поиск paymentId (через лог payment-service)
echo -e "\n${YELLOW}3. Поиск paymentId из логов payment-service...${NC}"
PAYMENT_ID=$(docker-compose logs payment-service 2>/dev/null | grep -o 'Payment created with Id [0-9]*' | grep -o '[0-9]*' | tail -1)
if [ -n "$PAYMENT_ID" ]; then
    echo -e "${GREEN}Найден paymentId: $PAYMENT_ID${NC}"
else
    echo -e "${RED}Не удалось найти paymentId в логах.${NC}"
    echo "  Проверьте, что payment-service работает и заказ был создан."
    exit 1
fi

# 4. Получение информации о платеже
echo -e "\n${YELLOW}4. Получение информации о платеже...${NC}"
PAYMENT_INFO=$(curl -s "$PAYMENT_URL/api/payments/get/$PAYMENT_ID")
PAYMENT_STATUS=$(echo "$PAYMENT_INFO" | jq -r '.status')
if [ "$PAYMENT_STATUS" = "false" ]; then
    echo -e "${GREEN}Платёж создан, статус: не оплачен (false)${NC}"
else
    echo -e "${RED}Неожиданный статус платежа: $PAYMENT_STATUS${NC}"
fi

# 5. Симуляция успешной оплаты
echo -e "\n${YELLOW}5. Обновление статуса платежа на оплачен...${NC}"
UPDATE_RESPONSE=$(curl -s -X PUT "$PAYMENT_URL/api/payments/updateStatus/$PAYMENT_ID/1")
if echo "$UPDATE_RESPONSE" | grep -q "Status updated"; then
    echo -e "${GREEN}Статус платежа обновлён на 'оплачен'${NC}"
else
    echo -e "${RED}Ошибка обновления статуса: $UPDATE_RESPONSE${NC}"
    exit 1
fi

# 6. Ожидание обработки события и отправки уведомления
echo -e "\n${YELLOW}6. Ожидание обработки события в NotificationService (5 сек)...${NC}"
sleep 5

NOTIFICATION_LOG=$(docker-compose logs notification-service 2>/dev/null | grep -E "Received payment event for Order|Sending payment notification to group" | tail -1)
if [ -n "$NOTIFICATION_LOG" ]; then
    echo -e "${GREEN}NotificationService обработал событие и отправил уведомление${NC}"
    echo "  $NOTIFICATION_LOG"
else
    echo -e "${RED}NotificationService не получил событие или не отправил уведомление. Проверьте Kafka и SignalR.${NC}"
    docker-compose logs --tail=20 notification-service
    exit 1
fi

echo -e "\n${GREEN}=== Интеграционный тест успешно завершён ===${NC}"
echo -e "Вы можете проверить уведомления через браузер, открыв sub.htm и подписавшись на test@example.com"

# Ожидание нажатия клавиши (если скрипт запущен двойным щелчком)
read -p "Нажмите Enter для выхода..."
