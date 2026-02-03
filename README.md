# ZoomClone Blazor Solution

Dette repository indeholder en Blazor-løsning med tre projekter:

- **Client**: Blazor WebAssembly frontend
- **Server**: ASP.NET Core WebAPI backend
- **Shared**: Delte modeller og logik mellem Client og Server

> ⚠️ Bemærk: `appsettings.json` er **ikke inkluderet** i repo’et af sikkerhedsmæssige årsager. Du skal selv oprette dem lokalt for både Client og Server.

---

## Forudsætninger

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) eller [VS Code](https://code.visualstudio.com/)
- SQLite (valgfrit, afhængigt af din `DbConnection`)

---

## Opsætning

1. **Klon repository**

```bash
git clone https://github.com/<your-username>/<repository-name>.git
cd <repository-name>
```

2. **Opret `appsettings.json`**

Opret `appsettings.json` i både **Client** og **Server** projekterne.  
Som reference skal de indeholde de nødvendige konfigurationer for:

- **Server**: ConnectionStrings, JWT, Client BaseAddress, Twilio, Logging
- **Client**: Logging, HttpClient BaseAddress

Eksempel:

```text
<Server>/appsettings.json
<Client>/wwwroot/appsettings.json
```

> Sørg for at tilpasse værdierne efter din lokale udviklingsmiljø (ports, JWT keys, Twilio nøgler mv.).

---

## Kørsel af projektet

### 1. Start Server

Naviger til Server-projektet og kør:

```bash
dotnet run --project Server/Server.csproj
```

Serveren vil starte på den port, der er angivet i `appsettings.json` (f.eks. `https://localhost:7298`).

### 2. Start Client

Naviger til Client-projektet og kør:

```bash
dotnet run --project Client/Client.csproj
```

Clienten vil køre på den port, der er angivet i `appsettings.json` (f.eks. `https://localhost:7128`).

### 3. Brug løsningen

- Åbn browseren og gå til Client URL (`https://localhost:7128`)
- Clienten vil kommunikere med Serveren via API.

---

## Database

Dette projekt bruger SQLite til lokal udvikling.

- Connection string konfigureres i Server `appsettings.json`:
  
```json
"ConnectionStrings": {
  "DbConnection": "Data Source=ZoomClone.db"
}
```

- Database vil blive oprettet automatisk ved første kørsel, hvis den ikke findes.

---


## Teknologier

- Blazor WebAssembly
- ASP.NET Core WebAPI
- Entity Framework Core (SQLite)
- JWT Authentication
- Twilio API

---
## Design Patterns og Arkitektur

Dette projekt anvender følgende patterns:

- Vertical Sliced Layer Architecture

- CQRS Pattern

- Mediator Design Pattern

