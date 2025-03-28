# CryptoConnector

CryptoConnector - это проект, состоящий из нескольких компонентов для работы с криптовалютными API.
Проект включает в себя:

- **CryptoConnector** - библиотека классов для работы с WebSocket, REST API и конвертацией баланса.
- **CryptoConnectorWebApi** - веб-приложение, предоставляющее REST API для получения данных о криптовалютах.
- **CryptoConnectorWPF** - WPF-приложение для отображения данных о балансе и WebSocket-сообщениях в реальном времени.

## Возможности
- Подключение к WebSocket для получения торговых данных.
- Взаимодействие с REST API для получения информации о криптовалютах.
- Конвертация баланса в различные валюты.
- Визуализация данных в WPF-приложении.

## Установка и запуск

### 1. Клонирование репозитория
```
git clone https://github.com/Byvaet0/CryptoConnector.git
cd CryptoConnector
```

### 2. Сборка проекта
Открыть решение в **Visual Studio** и собрать проект.

### 3. Запуск Web API
Запустить **CryptoConnectorWebApi** из Visual Studio или выполнить команду:
```
dotnet run --project CryptoConnectorWebApi
```

### 4. Запуск WPF-приложения
Запустить **CryptoConnectorWPF** через Visual Studio или выполнить:
```
dotnet run --project CryptoConnectorWPF
```

## Использование
- Веб-приложение предоставляет API для работы с криптовалютными данными.
- WPF-приложение подключается к WebSocket и отображает данные в реальном времени.



