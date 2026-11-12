# Demo rerun

**Status:** approved

## Goal
Make `demo` give the same result on every run, on fresh or used data.

## Behavior
- AC1: `docker compose run --rm demo` on a fresh volume exits 0.
- AC2: A second `docker compose run --rm demo` on the same volume, with no restart between, exits 0.
- AC3: A third consecutive `docker compose run --rm demo` exits 0.
- AC4: Each run saves under an id no earlier run used.
- AC5: Each run's `GET .../versions` check expects exactly `[1,2]`.
- AC6: `demo` leaves documents from earlier runs unchanged in the volume.
- AC7: `demo` still exits non-zero when any check fails (spec 003 AC15).
- AC8: `README.md` demo command stays `docker compose run --rm demo`.

## Non-goals
Wiping or recreating the volume. Delete or cleanup endpoints. Any API change. A `demo verify` mode. Changes to `tests`. Overrides spec 003 non-goal "demo reruns on existing data".
