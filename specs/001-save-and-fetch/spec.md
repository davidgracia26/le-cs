# Save and fetch documents

**Status:** approved

## Goal
Save any JSON document under a collection and id, fetch it back, and list ids in a collection.

## Behavior
- AC1: `PUT /books/1` with body `{"title":"Dune"}` -> 200, body `{"version":1}`.
- AC2: `GET /books/1` after AC1 -> 200, `Content-Type: application/json`, body `{"title":"Dune"}`.
- AC3: After AC1, file `books/1.jsonl` exists under the data directory with one line equal to `{"title":"Dune"}`.
- AC4: `PUT` of each non-object body `[1,2]`, `"text"`, `42`, `null` -> 200. `GET` returns the same JSON.
- AC5: `PUT {"s":"é<&>日本"}` then `GET` -> same text, with no `\u` escapes added.
- AC6: `PUT {"b":1,"a":2}` then `GET` -> `{"b":1,"a":2}` (key order kept).
- AC7: `PUT` with `Content-Type: text/plain` -> 200.
- AC8: `PUT` with no `Content-Type` -> 200.
- AC9: `GET /books/missing` -> 404.
- AC10: `GET /books` after saving ids `1` and `2` -> 200, `Content-Type: application/json`, body `["1","2"]` (ordinal sort).
- AC11: `GET /nothing`, a collection with no documents -> 200, body `[]`.
- AC12: `PUT /books/1` with invalid JSON -> 400.
- AC13: `PUT /books/1` with an empty body -> 400.
- AC14: `PUT /bad.name/1` -> 400.
- AC15: `PUT /books/bad.id` -> 400.
- AC16: `GET /bad.name` -> 400.
- AC17: `GET /bad.name/1` -> 400.
- AC18: `PUT /Books_2/doc-1` -> 200.
- AC19: `PUT` with a 100-character id -> 200.
- AC20: `PUT` with a 101-character id -> 400.
- AC21: `PUT` with a 100-character collection -> 200.
- AC22: `PUT` with a 101-character collection -> 400.
- AC23: `POST /books/1` with body `{"title":"Dune"}` -> 200, body `{"version":1}`.
- AC24: `GET /books/1` after AC23 -> 200, body `{"title":"Dune"}`.
- AC25: `GET /weatherforecast/1` -> 404 (template leftovers removed).

## Non-goals
Version history (spec 002). Containers and persistence (spec 003). Auth, pagination, delete, search, UI, schema validation. Number-text edge cases (`1e400`, big integers) and deep nesting beyond what `System.Text.Json` accepts by default.
