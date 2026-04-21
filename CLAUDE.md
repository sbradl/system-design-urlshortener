# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build and Run

Run the full system with Docker Compose from the root:

```
docker compose up
```

## .NET Services (Shortener & Redirector)

Both services follow the same pattern. From `shortener/` or `redirector/`:

```
dotnet build
dotnet test
dotnet publish Shortener/Shortener.csproj -c Release
```

To run a single test by name:

```
dotnet test --filter "TestMethodName"
```

All warnings are treated as errors (`TreatWarningsAsErrors=true`), code style is enforced in build (`EnforceCodeStyleInBuild=true`), and nullable reference types are enabled. Tests use MSTest via `Microsoft.Testing.Platform`.

## Frontend (Angular)

From `frontend/`:

```
npm start        # dev server at localhost:4200
npm run build
npm run test     # Vitest
```

## Architecture

This is a URL shortener with these services wired together in `compose.yaml`:

- **Frontend** (Angular 21, port 4200 in dev / 80 in Docker): Single `Shortener` component using Angular signals and `httpResource`. POSTs to `http://localhost:5000/api/urls`.
- **Shortener Service** (ASP.NET Core, port 5000): Validates HTTPS-only URLs, generates a base62-encoded short code, saves the mapping, and returns the full shortened URL (`ShortenedUrlBase` + `/` + code).
- **Redirector Service** (ASP.NET Core, port 5001): Intended to resolve `/{short-code}` → redirect to the original URL. Currently a scaffold.
- **Kafka** (port 9092), **DynamoDB Local** (port 8000), **Redis**: Infrastructure defined in compose but not yet wired into the services.

### Shortener Service key abstractions

| Interface | Purpose |
|---|---|
| `ShortenerService` | Generates a base62 short code (delegates to `UniqueIdService`) |
| `UniqueIdService` | Produces a unique `ulong` ID |
| `UrlStore` | Persists and retrieves `shortCode → sourceUrl` mappings |

`ShortenerServiceImpl` encodes a `ulong` to base62 using characters `0–9 A–Z a–z`. The `UrlsController` owns validation (HTTPS only) and constructs the final shortened URL from `ShortenedUrlBase` config + the generated code.

CORS is configured to allow `http://localhost` and `http://localhost:4200`.
