# VoteIt
Vote It !

- ~~[Trello Kanban](https://trello.com/b/0vR0ujR0/voit-it)~~

- [GitHub Project](https://github.com/knight720/VoteIt/projects/1)

## How to 
- ~~SQL Server for Docker~~ 
```powershell
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=[?]' -e 'MSSQL_PID=Express' -p 1433:1433 -v D:\Docker\VoteItDB:/var/opt/mssql -d mcr.microsoft.com/mssql/server:latest
```

- Entity Framework Core DB First
```powershell  
Scaffold-DbContext "Server=localhost;~Database=VoteItDB;User ID=[?];Password=[?];" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Tables Feed FeedLike -force  
```

- [建立 LocalDB Instance](https://docs.microsoft.com/zh-tw/sql/tools/sqllocaldb-utility?view=sql-server-2017)  
"套件管理器組控台" 執行 
```powershell
sqllocaldb create MSSQLLocalDB
```

- 重建 VoteItDB  
"SQL Server 物件總管"連線至 (localdb)\\MSSQLLocalDB  
執行 SQL 指令建立資料庫 
```powershell
conf/VoteItDB.sql  
```

- [重建 ApplicationDB](https://docs.microsoft.com/zh-tw/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-2.2&tabs=visual-studio)  
"套件管理器組控台" 執行 
```powershell
Update-Database -Context ApplicationDbContext  
```

## Rference
- [reddit](https://zh.wikipedia.org/wiki/Reddit)

- [folder structure](https://stackoverflow.com/questions/446017/popular-folder-structure-for-build)

- [dotnet folder structure](https://github.com/dotnet/project-system)

- Microsoft Docs

    - [Connection Strings](https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-strings)

    - [Secret Manager(開發階段)](https://docs.microsoft.com/zh-tw/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows)  
        ```powershell
        %APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json
        ```

    - [[ASP.NET Core] Identity - Part 1 使用 Google 登入](https://blog.kevinyang.net/2018/05/31/aspnet-core-identity/)

    - [HttpClientFactory](https://docs.microsoft.com/zh-tw/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)

    - [在 ASP.NET Core 中使用託管服務的背景工作](https://docs.microsoft.com/zh-tw/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-2.2)
    Docs/aspnetcore/fundamentals/host/hosted-services/samples/2.x/BackgroundTasksSample-WebHost/

    - [在微服務中使用 IHostedService 和 BackgroundService 類別實作背景工作](https://docs.microsoft.com/zh-tw/dotnet/standard/microservices-architecture/multi-container-microservice-net-applications/background-tasks-with-ihostedservice)

- [SVG Icon](https://www.flaticon.com/)

- Bootstrap

    - [Modal (popup tooltip)](https://getbootstrap.com/docs/4.0/components/modal/)  

- Slack API

    - [Your Apps](https://api.slack.com/apps)

    - [Incoming Webhooks](https://api.slack.com/apps/AEU8K3B3L/incoming-webhooks?success=1)

    - [Deep linking into Slack clients](https://api.slack.com/docs/deep-linking)  
    由 Web 開啟 Slack Channel

- Google

    - [Google API Console](https://console.developers.google.com)

- ASP.NET Core

    - [Cannot Consume Scoped Service From Singleton – A Lesson In ASP.net Core DI Scopes](https://dotnetcoretutorials.com/2018/03/20/cannot-consume-scoped-service-from-singleton-a-lesson-in-asp-net-core-di-scopes/)