# telegram_bot - сервер для телеграм-бота для взаимодействия администраторов и клиентов

## Для запуска:
В корне проекта создаем файл appsettings.json, в котором указываем строку подключения для БД
Далее в файле ./TelegramBot/Services/BotService.cs: 

Вставьте свои токены для группы и бота;

В методе GetContext():
```csharp
options.UseNpgsql("Your connection string");
```


Затем:
```bash
docker run --name telegram -p 5432:5432 -e POSTGRES_PASSWORD=your_password postgres
```
```bash
cd TelegramBot
dotnet build
dotnet run
```

## Функционал
Данный бот при начале диалога создает в группе администраторов топик с никнеймом пользователя, написавшего боту, затем пересылает это сообщение в топик. Администратор, состоящий в группе, может ответить пользователю
в соответсвующем топике, пользователь получит это сообщение в диалоге с ботом.
Таким образом, сокращается непосредственное взаимодействие администраторов и пользователей, а также система топиков позволяет структурировать сообщения от клиентов.