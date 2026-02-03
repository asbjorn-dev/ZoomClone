Blazor ZoomClone Solution
Denne løsning er en Blazor WebAssembly Hosted applikation bestående af en Client, Server og Shared projektstruktur.

Forudsætninger
.NET SDK

En IDE (Visual Studio 2022, VS Code eller Rider)

SQLite (da din database bruger en .db fil)

Konfiguration
Projektet bruger appsettings.json til konfiguration, men disse er udeladt fra versionsstyring (via .gitignore). Du skal oprette dem manuelt:

1. Server-konfiguration
Opret /Server/appsettings.json og indsæt følgende. Vigtigt: Udskift Twilio-oplysningerne med dine egne, hvis du ikke bruger test-credentials.

JSON
{
  "ConnectionStrings": {
    "DbConnection": "Data Source=ZoomClone.db"
  },
  "Jwt": {
    "Key": "DIN_HEMMELIGE_KEY_HER",
    "Issuer": "https://localhost:7298",
    "Audience": "https://localhost:7298",
    "ExpireMinutes": 120
  },
  "Client": {
    "BaseAddress": "https://localhost:7128"
  },
  "Twilio": {
    "AccountSid": "DIT_ACCOUNT_SID",
    "ApiKey": "DIN_API_KEY",
    "ApiSecret": "DIN_API_SECRET",
    "AuthToken": "DIT_AUTH_TOKEN"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
    }
  },
  "AllowedHosts": "*"
}

2. Client-konfiguration
Opret /Client/wwwroot/appsettings.json:

JSON
{
  "HttpClient": {
    "Name": "ApiClient",
    "BaseAddress": "https://localhost:7298"
  },
  "Token": {
    "Key": "token"
  }
}

Opstart af projektet
Da dette er en hosted løsning, skal du altid starte Server-projektet, som derefter vil servere din Blazor Client.

Visual Studio
Sæt Server som "Startup Project".

Tryk F5.

Terminal
Bash
dotnet run --project Server
🏗 Database (Entity Framework)
Hvis du har ændret i modellerne i Shared eller Server, skal du opdatere din SQLite database:

dotnet ef migrations add InitialCreate --project Server

dotnet ef database update --project Server
