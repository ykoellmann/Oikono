# Oikono - Vereinfachte Minimal API Version

Dies ist eine stark vereinfachte Version des Original-Oikono-Projekts, basierend auf ASP.NET Core Minimal APIs.

## Unterschiede zum Original-Projekt

### Architektur
- **Minimal API** statt Controller-basierter Ansatz
- **EF Core direkt in Endpoints** statt separate Repositories
- **Einfache GUIDs** statt strongly-typed ID-Objekte
- **Keine Clean Architecture Schichten** (Domain, Application, Infrastructure)
- **Kein CQRS/MediatR** - Logik direkt in Endpoints
- **Keine Specifications** - EF Core Queries direkt
- **Keine Value Objects** - primitive Typen

### Was beibehalten wurde
- Alle Entities und deren Beziehungen
- JWT Authentication mit Refresh Tokens
- Volle CRUD-Funktionalität für Rezepte
- Asset Management (Bild-Upload/Download)
- PostgreSQL als Datenbank
- Swagger/OpenAPI Dokumentation

## Projektstruktur

```
Oikono/
├── Data/                   # DbContext
│   └── OikonoDbContext.cs
├── DTOs/                   # Request/Response DTOs
│   ├── Auth/
│   ├── Recipe/
│   └── Common/
├── Endpoints/              # Minimal API Endpoints
│   ├── AuthEndpoints.cs
│   ├── RecipeEndpoints.cs
│   ├── IngredientEndpoints.cs
│   ├── DeviceEndpoints.cs
│   ├── TagEndpoints.cs
│   ├── SideDishEndpoints.cs
│   └── AssetEndpoints.cs
├── Entities/               # Domain Entities
│   ├── User.cs
│   ├── RefreshToken.cs
│   ├── Recipe.cs
│   ├── Part.cs
│   ├── PartIngredient.cs
│   ├── Step.cs
│   ├── Ingredient.cs
│   ├── Device.cs
│   ├── Tag.cs
│   ├── SideDish.cs
│   ├── RecipeTag.cs
│   ├── RecipeSideDish.cs
│   ├── RecipeAsset.cs
│   └── UnitType.cs
├── Services/               # Services
│   ├── JwtService.cs
│   └── CurrentUserService.cs
└── Program.cs              # Application Startup
```

## Verfügbare Endpoints

### Authentication (`/api/auth`)
- `POST /api/auth/register` - Benutzer registrieren
- `POST /api/auth/login` - Login (JWT + Refresh Token)
- `POST /api/auth/refresh` - Token erneuern

### Recipes (`/api/recipes`)
- `GET /api/recipes` - Alle Rezepte (mit Pagination, Search)
- `GET /api/recipes/{id}` - Rezept-Details
- `POST /api/recipes` - Rezept erstellen
- `GET /api/recipes/units` - Verfügbare Einheiten

### Ingredients (`/api/ingredients`)
- `GET /api/ingredients` - Alle Zutaten
- `GET /api/ingredients/{id}` - Zutat nach ID
- `POST /api/ingredients` - Zutat erstellen
- `PUT /api/ingredients/{id}` - Zutat aktualisieren
- `DELETE /api/ingredients/{id}` - Zutat löschen

### Devices (`/api/devices`)
- CRUD-Operationen für Küchengeräte

### Tags (`/api/tags`)
- CRUD-Operationen für Tags

### Side Dishes (`/api/sidedishes`)
- CRUD-Operationen für Beilagen

### Assets (`/api/assets`)
- `GET /api/assets/{id}` - Bild herunterladen
- `POST /api/assets/recipe` - Bild zu Rezept hochladen

## Einrichtung

### Voraussetzungen
- .NET 9.0 SDK
- PostgreSQL Datenbank

### Konfiguration

1. **Datenbank-Verbindung** in `appsettings.json` anpassen:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=oikono;Username=postgres;Password=postgres"
  }
}
```

2. **JWT-Konfiguration** (Optional - Standard ist bereits gesetzt):
```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLongForHS256",
    "Issuer": "Oikono",
    "Audience": "OikonoUsers"
  }
}
```

### Datenbank-Migration

```bash
# Migration erstellen
dotnet ef migrations add InitialCreate

# Datenbank aktualisieren
dotnet ef database update
```

### Anwendung starten

```bash
dotnet run
```

Die API ist dann verfügbar unter:
- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `https://localhost:5001/swagger`

## Authentifizierung

### Registrierung
```bash
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "Max",
  "lastName": "Mustermann",
  "email": "max@example.com",
  "password": "SecurePassword123!",
  "birthDate": "1990-01-01"
}
```

### Login
```bash
POST /api/auth/login
Content-Type: application/json

{
  "email": "max@example.com",
  "password": "SecurePassword123!"
}
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "...",
  "expiresAt": "2025-11-17T12:00:00Z"
}
```

### Authentifizierte Requests

Füge den JWT Token im Authorization Header hinzu:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

## Rezept erstellen (Beispiel)

```bash
POST /api/recipes
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Spaghetti Carbonara",
  "portions": 4,
  "calories": 650,
  "rating": 5,
  "tagIds": ["{tag-guid}"],
  "sideDishIds": ["{sidedish-guid}"],
  "parts": [
    {
      "name": "Sauce",
      "ingredients": [
        {
          "ingredientId": "{ingredient-guid}",
          "amount": 200,
          "unit": 2
        }
      ]
    }
  ],
  "steps": [
    {
      "description": "Wasser zum Kochen bringen",
      "duration": "00:10:00",
      "temperature": 100,
      "deviceId": "{device-guid}"
    }
  ]
}
```

## Unit Types (Einheiten)

```
0 = Milliliter
1 = Liter
2 = Gram
3 = Kilogram
4 = Centimeter
5 = Meter
6 = Teaspoon
7 = Tablespoon
8 = Piece
```

## CORS-Konfiguration

Standardmäßig ist CORS für `http://localhost:5173` (Frontend) aktiviert. Passe dies in `Program.cs` an, falls nötig.

## Fehlende Features (im Vergleich zum Original)

- Rollen/Permissions System (vereinfacht - keine separate User-Rollen)
- Rate Limiting
- Redis Caching
- Idempotency Keys
- Policy-basierte Authorization
- Hangfire Background Jobs
- Serilog Structured Logging
- Update/Delete für Rezepte
- Mapster/AutoMapper (manuelles Mapping)

## Vorteile dieser Version

- **Einfacher zu verstehen** - Weniger Abstraktionen
- **Schneller zu entwickeln** - Direkter Code-Flow
- **Weniger Boilerplate** - Keine generischen Repositories
- **Leichter zu warten** - Weniger Schichten
- **Perfekt für kleinere Projekte** oder Prototypen

## Nächste Schritte

1. Datenbank erstellen und migrieren
2. Testdaten erstellen (Ingredients, Devices, Tags, SideDishes)
3. Frontend anbinden
4. Optional: Zusätzliche Features nach Bedarf hinzufügen
