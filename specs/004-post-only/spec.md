# Post only

**Status:** approved

## Goal
Save documents with `POST` only and remove the `PUT` verb.

## Behavior
- AC1: `PUT /books/1` with body `{"title":"Dune"}` -> 405.
- AC2: `PUT /books/1` on an existing document -> 405, and `GET /books/1/versions` returns the same list as before.
- AC3: `POST /books/1` keeps its behavior from specs 001 and 002 (200, `{"version":n}`, one new version per call).
- AC4: Every automated test saves with `POST`, except the tests for AC1 and AC2.
- AC5: `docker compose run --rm tests` exits 0.
- AC6: `demo` saves with `POST` and exits 0 on a fresh volume.
- AC7: `AGENTS.md` and `README.md` contain no `PUT` reference.

## Non-goals
Rewriting specs 001-003 and their task lists: they stay as history, and their `PUT` criteria now read as `POST`. An `Allow` header check on the 405. Idempotency, dedupe, or replace semantics. Any change to storage, paths, status codes, or the version format.
