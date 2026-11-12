# Version history

**Status:** approved

## Goal
Keep every prior content of a document and make each version retrievable.

## Behavior
- AC1: Second `PUT /books/1` with a different body -> 200, body `{"version":2}`.
- AC2: `GET /books/1` after two PUTs -> returns the second body.
- AC3: `GET /books/1/versions` after three PUTs -> 200, `Content-Type: application/json`, body `[1,2,3]`.
- AC4: `GET /books/1/versions/1` -> 200, `Content-Type: application/json`, body equals the first PUT body.
- AC5: `GET /books/1/versions/2` -> 200, body equals the second PUT body.
- AC6: `POST /books/1` on an existing document -> 200, body `{"version":2}`.
- AC7: `PUT` of a body identical to the latest -> 200, adds a new version (no dedupe).
- AC8: `GET /books/1/versions/9` when only 3 exist -> 404.
- AC9: `GET /books/1/versions/0` -> 404.
- AC10: `GET /books/1/versions/abc` -> 404.
- AC11: `GET /books/missing/versions` -> 404.
- AC12: `GET /books/missing/versions/1` -> 404.
- AC13: `GET /bad.name/1/versions` -> 400.
- AC14: `GET /books/bad.id/versions/1` -> 400.
- AC15: A PUT on one document leaves other documents' versions unchanged.
- AC16: A failed `PUT` (400) adds no version.
- AC17: After n versions, the document file holds exactly n lines and ends with a newline.
- AC18: 50 concurrent `PUT`s of distinct bodies to one document -> all return 200, `GET .../versions` returns `[1..50]`, and every version is valid JSON equal to one sent body.

## Non-goals
Version timestamps, authors, diffs, rollback, delete, version limits. Dedupe and normalization of any kind: every PUT adds a version. Idempotency keys. `If-Match`. Recovery from a crash mid-write. Exhaustive invalid version-number cases (`-1`, `01`, `+1`, overflow).
