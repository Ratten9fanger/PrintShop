# PrintShop

REST API для интернет-магазина полиграфической продукции.

## Стек

- .NET 8, ASP.NET Core, C#
- Entity Framework Core, PostgreSQL
- Apache Kafka (интеграция с сервисом уведомлений)
- xUnit (unit + integration tests)
- Docker, Docker Compose

## Архитектура

Clean Architecture с разделением на слои:
- `PrintShop.API` — контроллеры, endpoints
- `PrintShop.Application` — бизнес-логика, CQRS (MediatR)
- `PrintShop.Domain` — сущности, value objects
- `PrintShop.DataAccess` — EF Core, репозитории
- `PrintShop.Infrastructure` — внешние зависимости
- `PrintShop.NotificationService` — отдельный сервис для отправки уведомлений
- `PrintShop.Tests` — тесты

## Что реализовано

- CRUD для товаров и категорий
- Создание и управление заказами
- JWT авторизация
- Интеграция с Kafka: при создании заказа публикуется событие в топик, NotificationService его потребляет и отправляет уведомление
- Покрытие тестами (unit + integration через WebApplicationFactory)

## Запуск

```
docker-compose up --build
```

API будет доступен на http://localhost:5000/swagger.

## Тесты

dotnet test
