# AGENTS.md

Strict negative constraints:
- Use Plain Direct English (active voice, imperative mood, anglo-saxon words)
- Every word should be load bearing
- Treat every query as a direct programming execution. 
- ABSOLUTELY ZERO conversational filler, robotic pleasantries, pleasantries, or introductory preambles.
- DO NOT restate, paraphrase, or acknowledge the prompt. 
- DO NOT write a concluding summary, a "Let me know if you need help", or a closing paragraph.

Structural enforcement:
- Output the direct answer or solution starting on character 1, line 1 of the response.
- If the response requires multiple points, use a markdown list with bold headers (e.g., "**Key Factor:** Detail").
- If the answer is code, output ONLY the code block. No explanation before or after.

## Agent config

- Treat `.claude/` as the primary location for skills (`.claude/skills/`) and hooks (`.claude/hooks/`). Look there first. Write new skills and hooks there.
- Spec flow: `spec` -> `tasks` -> `work`. Artifacts live in `specs/<slug>/`.
- Keep `specs/<slug>/scratchpad.md` during a cycle. Note skill, hook, and rule improvements as they appear. After the last task, apply them to `.claude/skills/`, `.claude/hooks/`, `.claude/settings.json`, and `AGENTS.md`. Then delete the scratchpad, commit the edits as the spec's final commit (same `NNN:` prefix), and report what changed.

## Project

One-hour interview exercise: a **document store** service (`GUIDANCE.md`, `briefs/document-store.md`).
Stack: .NET 9 (ASP.NET Core minimal API, C#), Docker Compose.

Build the smallest clear solution. Scope is graded. Stop when the three features work in a container and tests prove it.

## Required behavior

1. Save a JSON document under `collection` and `id`. Accept any shape. Do not inspect or validate it.
2. Fetch a document by collection and id. List ids in a collection.
3. Keep prior content on update. Make every prior version retrievable.

## Out of scope

Do NOT build: auth, pagination, delete, search/filter/query, UI, real database, schema validation, caching, rate limiting, Swagger/OpenAPI, migrations, CI config, structured logging, metrics.

## Hard rules

- **Docker:** Assume only Docker with Compose. No local .NET SDK, `make`, `curl`, or shell. Build, run, and test inside containers.
- **Start:** `docker compose up` starts the service.
- **Proof:** Ship automated tests and a demo script, both run through Docker. `tests` runs `dotnet test`. `demo` runs `demo/demo.sh` over HTTP against `api`.
- **README:** List the exact start and proof commands. No design docs, trade-off memos, or summaries.
- **Commits:** Commit small and often. History is read.
- **Origin:** Write fresh code. Reuse nothing.

## Layout

```
LE.CS.sln
LE.CS.Api/                minimal API (Program.cs, store)
LE.CS.Tests/              xUnit, WebApplicationFactory API tests
Dockerfile                multi-stage: sdk builds, aspnet runs (to add)
docker-compose.yml        `api`, `tests`, and `demo` services (to add)
demo/demo.sh              curl script run by `demo` in `curlimages/curl` (to add)
.gitignore, .dockerignore ignore bin/ obj/ (to add)
.gitattributes            `*.sh text eol=lf` so Windows checkouts keep `demo.sh` runnable in the container
README.md                 exact start and proof commands only
GUIDANCE.md               the exercise rules from the interviewer (read-only)
```

Keep project names. Do not restructure.

## Template leftovers (remove)

- `Program.cs`: delete `/weatherforecast`, `WeatherForecast`, `AddOpenApi`, `MapOpenApi`, `UseHttpsRedirection`.
- `LE.CS.Api.csproj`: drop `Microsoft.AspNetCore.OpenApi`.
- Delete `LE.CS.Api.http`.
- `LE.CS.Tests`: replace `UnitTest1.cs` with real tests. Drop `coverlet.collector`. Add `Microsoft.AspNetCore.Mvc.Testing`.
- `Program.cs`: add `public partial class Program { }` for `WebApplicationFactory<Program>`.
- Never commit `bin/` or `obj/`.
- Ignore empty root `data/`. Runtime `DATA_DIR` is `/data`.

## .NET conventions

- Both projects target `net9.0`. Pin `mcr.microsoft.com/dotnet/sdk:9.0` and `aspnet:9.0`. Change project and images together.
- Use minimal APIs, nullable reference types, implicit usings, file-scoped namespaces. Namespaces match project names.
- Treat documents as `JsonElement` or raw JSON. Never bind a typed model.
- Hide storage behind `IDocumentStore` with a JSONL file implementation. Register via DI.
- Use one API project and one test project. Add no libraries or layers.
- Write compact code. Skip generic repositories, MediatR, AutoMapper.
- Add no NuGet packages beyond `Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio`, `Microsoft.AspNetCore.Mvc.Testing`.
- Do not add or remove comments unless asked.

## Storage

- Write one file per document: `{DATA_DIR}/{collection}/{id}.jsonl`. One compact line per version. Update appends a line. Store the body as sent, compacted. Every PUT/POST adds a version, even when the body is identical to the last. Do not dedupe or normalize.
- Latest version is the last line. Version *n* is line *n*. List ids by listing files in the collection folder.
- Read `DATA_DIR` from config/env. Default `/data`. Tests use a temp directory.
- Mount `/data` as a **named volume** in compose. No bind mount.
- Runtime images run as non-root `app`. In the Dockerfile, create `/data` and `chown` it to `app`. Fresh named volumes fail writes otherwise.
- Allow only `[A-Za-z0-9_-]`, 1 to 100 characters, in `collection` and `id`. Return 400 otherwise. Blocks path traversal and over-long filenames.
- Write each version as one line plus `\n` in a single append. Use `UnsafeRelaxedJsonEscaping`. Write `JsonElement` values so number text and key order stay unchanged.
- Accept PUT bodies with any `Content-Type`. Read the raw body.
- Accept version `n` only if it matches `^[1-9][0-9]*$` and fits in an `int`. Return 404 otherwise.
- Lock per document (`SemaphoreSlim`). Concurrent PUTs must not interleave lines.

## API

- `PUT /{collection}/{id}` - save document (any JSON body). Keep the prior version. `POST` on the same route does the same (the brief says "POST a document").
- `GET /{collection}/{id}` - latest version. 404 if missing.
- `GET /{collection}` - list ids.
- `GET /{collection}/{id}/versions` - list versions.
- `GET /{collection}/{id}/versions/{n}` - one version.

Return 400 for invalid JSON and invalid collection/id. Check collection and id (400) before the version number (404). All GET 200 responses use `Content-Type: application/json`.

## Commands

```
docker compose up --build            # start service
docker compose run --rm tests        # run tests
docker compose run --rm demo         # demo over HTTP (starts api)
docker compose down                  # stop, keep volume
```

Host port is `${PORT:-8080}`.

Set `pull_policy: build` on `tests`; otherwise `docker compose run` reuses a stale image. Run `docker compose --progress quiet run --rm tests` for short output.

Update when compose services are final.

## Verify before finishing

- `docker compose up --build` starts clean from a fresh clone.
- Tests/demo pass through Docker only.
- Data survives `docker compose down` / `up`. No permission errors.
- No out-of-scope feature exists.
- README commands match real commands.
- `git ls-files` lists no `bin/` or `obj/` path.
- `docker compose up` starts `api` only. Put `tests` and `demo` under a compose `profiles` entry.
