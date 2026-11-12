# Container and proof

**Status:** approved

## Goal
Run the service and its proof with Docker alone, with data that survives restarts.

## Behavior
- AC1: `docker compose up --build` on a fresh clone starts the API on host port 8080. `PORT=9090 docker compose up` moves the host port.
- AC2: `docker compose up` starts the `api` service only. The `tests` and `demo` services stay stopped.
- AC3: `PUT http://localhost:8080/books/1` against the running container -> 200.
- AC4: `docker compose run --rm tests` runs the full test suite and exits 0 when all pass.
- AC5: After `docker compose down` then `docker compose up`, `GET /books/1` returns the last saved body and `GET /books/1/versions` returns every earlier version number.
- AC6: First `PUT` on a fresh named volume succeeds as the non-root user (no permission error).
- AC7: A new app instance over the same data directory returns documents saved by the prior instance (automated test).
- AC8: `DATA_DIR` unset -> the service writes under `/data`.
- AC9: `DATA_DIR` set -> the service writes under that path (automated test).
- AC10: `README.md` lists the exact start command and the exact proof commands, and each runs as written.
- AC11: `docker compose run --rm demo` starts `api` if it is stopped and waits until `api` accepts connections.
- AC12: `demo` sends `PUT` to `api` over the compose network at `http://api:8080`.
- AC13: `demo` checks `GET` of the latest body after two `PUT`s.
- AC14: `demo` checks `GET .../versions` lists versions 1 and 2, and `GET .../versions/1` returns the first body.
- AC15: `demo` exits 0 when every check passes and non-zero when any check fails.
- AC16: `.gitignore` and `.dockerignore` list `bin/` and `obj/`. `git ls-files` shows no `bin/` or `obj/` path.

## Non-goals
HTTPS, health checks, CI config, logging setup, image publishing, a local .NET toolchain, any test beyond `demo` over a real port. A `demo verify` mode or demo reruns on existing data. Offline first run (images and NuGet packages need network).
