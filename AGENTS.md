# AGENTS.md

## Project

One-hour pre-interview exercise: a **document store** service (see `README.md` and `briefs/document-store.md`).
Stack: **.NET** (ASP.NET Core minimal API, C#), containerized with Docker Compose.

Optimize for a small, obvious, clean solution. Scope is part of what is being evaluated. When the three required features work, runs in a container, and is shown to work: **stop**.

## Required behavior

1. Save a JSON document under a `collection` and `id`. Any shape; do not inspect or validate it.
2. Fetch a document by collection and id; list the ids in a collection.
3. Keep previous content when a document is updated, and make prior versions retrievable.

## Out of scope (do NOT build)

Authentication/authorization, pagination, delete, search/filtering/queries, a UI, a real database (in-memory or files on disk are fine), schema validation, caching, rate limiting, generated API docs (no Swagger/OpenAPI), migrations, CI config, structured logging, metrics.

If tempted to add one of these, stop.

## Hard rules (from README)

- **Docker (with Compose) is the only thing assumed on the target machine.** No local .NET SDK, `make`, `curl`, or particular shell. Build, run, and tests must all work inside containers.
- **One command brings it up** (`docker compose up`).
- **Must be shown to work**: automated tests and/or a demo script, runnable via Docker alone (e.g. a compose service that runs `dotnet test` or the demo).
- README must list the exact commands to start the service and run the proof. No design docs, trade-off memos, or summaries.
- Commit incrementally, the way a normal developer would. Commit history is read.
- Build fresh; no reuse of pre-existing code.

## Layout

Solution was scaffolded with `dotnet new` (webapi + xunit). Keep these names; do not rename or restructure.

```
LE.CS.sln
LE.CS.Api/                ASP.NET Core minimal API (Program.cs, store, models)
LE.CS.Tests/              xUnit tests (references LE.CS.Api; WebApplicationFactory for API-level tests)
briefs/document-store.md  the only brief (the other briefs were removed)
Dockerfile                multi-stage: sdk image to build, aspnet image to run (to add)
docker-compose.yml        `api` service + `tests` service (profile or separate target) (to add)
.dockerignore, .gitignore exclude bin/ obj/ (to add; neither exists yet)
README.md                 run + test instructions
```

## Scaffold cleanup (template leftovers to remove)

- `Program.cs` still contains the template `/weatherforecast` endpoint and `WeatherForecast` record: replace.
- `LE.CS.Api.csproj` references `Microsoft.AspNetCore.OpenApi` and `Program.cs` calls `AddOpenApi`/`MapOpenApi`: remove (OpenAPI is out of scope).
- `app.UseHttpsRedirection()`: remove (container serves plain HTTP; it would break tests/demo).
- `LE.CS.Api.http` and `Properties/launchSettings.json` (template, port 5146): delete the `.http` file; launch settings are not used in the container.
- `LE.CS.Tests/UnitTest1.cs` is an empty template test: replace with real tests.
- `LE.CS.Tests.csproj` has `coverlet.collector` (template default): remove unless wanted; add `Microsoft.AspNetCore.Mvc.Testing`.
- `Program.cs` uses top-level statements: add `public partial class Program { }` so `WebApplicationFactory<Program>` can see it.
- `bin/` and `obj/` are currently untracked and not ignored: never commit them.
- `data/` at repo root is an empty leftover directory, unrelated to runtime `DATA_DIR` (`/data` in the container).

## .NET conventions

- Both projects currently target `net9.0` (non-LTS). Use matching `mcr.microsoft.com/dotnet/sdk:9.0` / `aspnet:9.0` images, or retarget both projects and images to the current LTS together. Never mix versions. Use official `mcr.microsoft.com/dotnet/sdk` and `.../aspnet` images.
- Minimal APIs, nullable reference types enabled, implicit usings on, file-scoped namespaces.
- Treat documents as `System.Text.Json.JsonElement` / raw JSON; never bind to a typed model.
- Store behind a small interface (e.g. `IDocumentStore`) with a simple JSONL file-backed implementation (see Storage). Register via DI.
- Single API project (`LE.CS.Api`) plus one test project (`LE.CS.Tests`). No extra class libraries or layers.
- Root namespace follows the project (`LE.CS.Api`, `LE.CS.Tests`).
- Keep code compact; avoid extra layers, generic repositories, MediatR, AutoMapper, etc.
- Add no NuGet packages beyond the test stack (xUnit, `Microsoft.NET.Test.Sdk`, `xunit.runner.visualstudio`, `Microsoft.AspNetCore.Mvc.Testing`) unless clearly needed.
- Do not add or remove comments unless asked.

## Storage

- One JSONL file per document: `{DATA_DIR}/{collection}/{id}.jsonl`. Each line is one version, written compact on a single line. An update appends a line.
- Latest version = last line; version *n* = line *n*. List ids = list files in the collection folder.
- `DATA_DIR` comes from config/env var, default `/data`. Tests use a temp directory.
- In `docker-compose.yml`, mount `/data` as a **named volume** (not a bind mount) so data survives restarts and avoids Windows path/permission issues.
- The .NET runtime images run as non-root `app`. In the Dockerfile, create `/data` and `chown` it to `app` before the container starts, otherwise writes to a fresh named volume fail.
- `collection` and `id` become path segments: allow only `[A-Za-z0-9_-]` (or encode them) to prevent path traversal. Reject others with 400.
- Serialize writes per document (e.g. a lock or `SemaphoreSlim`) so concurrent PUTs cannot interleave lines.

## Suggested API shape

- `PUT /{collection}/{id}` - save/overwrite a document (body is arbitrary JSON); previous content is retained as a version.
- `GET /{collection}/{id}` - latest version (404 if missing).
- `GET /{collection}` - list of ids.
- `GET /{collection}/{id}/versions` - list previous versions; `GET /{collection}/{id}/versions/{n}` - a specific version.

Return 400 for invalid JSON bodies and for invalid collection/id values.

## Commands (all via Docker)

```
docker compose up --build                 # start the service
docker compose run --rm tests             # run automated tests (service name may vary)
```

Solution/test paths: `LE.CS.sln`, `LE.CS.Tests/`. Update this section when the compose services are finalized.

## Verification before finishing

- `docker compose up --build` starts cleanly from a fresh clone.
- Tests/demo pass through Docker only.
- Data persists across `docker compose down` / `up` (named volume works, no permission errors).
- Out-of-scope list above is still respected.
- README run instructions match the real commands.
