# TODO Web API

TODO Web API - это приложение для управления заметками,напоминаниями и тэгами.

## Описание проекта

Проект использует ASP.NET Core для создания Web API, а также MediatR для обработки команд и запросов. Все заметки хранятся в базе данных, и проект включает несколько команд и запросов для управления заметками,напоминаниями и тэгами.

## Требования

Для запуска этого проекта вам понадобятся:

- .NET 7 SDK или новее
- PostgreSQL или другая поддерживаемая база данных
- Среда разработки, такая как Visual Studio или Visual Studio Code

## Установка

1. **Клонируйте репозиторий:**
  git clone https://github.com/TV0Valde/TODO.git
    cd TODO
   
2. **Настройте строку подключения к базе данных:**
    Обновите строку подключения в `appsettings.json`

3. **Установите зависимости:**
    В корневой папке проекта выполните команду:  dotnet restore
4. **Примените миграции для создания базы данных:**
    dotnet ef database update
   
## Запуск

1. **Запустите приложение:**
dotnet run
 
    Или используйте Visual Studio, чтобы запустить проект.

2. **Откройте браузер и перейдите по адресу:**
 https://localhost:5001/swagger
 Здесь вы найдете интерфейс Swagger, который позволит вам тестировать API.
    
   

    

    

    
