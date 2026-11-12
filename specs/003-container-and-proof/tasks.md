# Tasks: Container and proof

- [x] T1 (AC4) (characterization) Run the suite through compose
  - **Test:** `docker compose run --rm tests` - builds the `tests` target and runs `dotnet test`.
  - **Done when:** green on arrival (exit 0); full suite green.

- [x] T2 (AC7) (characterization) Reopen a data directory with a new instance
  - **Test:** `StorageTests.New_Instance_Reads_Prior_Instance_Data` - give `ApiFixture` an optional `dataDir` argument. Instance A PUTs `/books/1` twice. Start instance B on A's `DataDir`. Assert B `GET /books/1` returns the second body and `GET /books/1/versions` is `[1,2]`.
  - **Done when:** green on arrival; full suite green.

- [x] T3 (AC9) (characterization) Write under the configured `DATA_DIR`
  - **Test:** `StorageTests.Data_Dir_Setting_Sets_Write_Path` - `ApiFixture` with a temp `DATA_DIR`, PUT `/books/1`, assert `{DataDir}/books/1.jsonl` exists.
  - **Done when:** green on arrival; full suite green.

- [x] T4 (AC8) (characterization) Default to `/data`
  - **Test:** `StorageTests.Unset_Data_Dir_Writes_Under_Slash_Data` - build `JsonlDocumentStore` from an empty `IConfiguration`, `Save` a unique collection and id, assert `/data/{collection}/{id}.jsonl` exists, then delete that collection folder. Runs in the `tests` container.
  - **Done when:** green on arrival; full suite green.

- [x] T5 (AC16) (characterization) Ignore `bin/` and `obj/`
  - **Test:** `git ls-files LE.CS.Api LE.CS.Tests` lists no `bin/` or `obj/` path, and `.gitignore` and `.dockerignore` each contain `bin/` and `obj/` lines (read both files).
  - **Done when:** green on arrival (no matching path, both lines present); full suite green.

- [x] T6 (AC1) Start the API on host port 8080
  - **Test:** `docker compose up --build -d`, then `docker compose port api 8080` prints `0.0.0.0:8080`; after `docker compose down`, `PORT=9090 docker compose up -d` and the same command print `0.0.0.0:9090`.
  - **Done when:** red (`no such service: api`), then green with an `aspnet:9.0` runtime stage in `Dockerfile` that runs as `app`, and an `api` service in `docker-compose.yml` with `ports: "${PORT:-8080}:8080"`; `docker compose down`; full suite green.

- [x] T7 (AC3) Save a document in the running container
  - **Test:** `docker compose up --build -d`, then `docker run --rm --network le-cs_default curlimages/curl -s -o /dev/null -w "%{http_code}" -X PUT -d "{}" http://api:8080/books/1` prints `200`.
  - **Done when:** red (`500`, write denied for `app`), then green with `/data` created and `chown`ed to `app` in the runtime stage and a named volume mounted at `/data`; full suite green.

- [x] T8 (AC6) (characterization) First PUT on a fresh named volume
  - **Test:** `docker compose down -v`, `docker compose up --build -d`, run the T7 curl command; prints `200`.
  - **Done when:** green on arrival (no permission error in `docker compose logs api`); full suite green.

- [x] T9 (AC5) (characterization) Keep data across `down` and `up`
  - **Test:** PUT `/books/1` twice with different bodies, `docker compose down`, `docker compose up -d`; curl `GET /books/1` returns the second body and `GET /books/1/versions` returns `[1,2]`.
  - **Done when:** green on arrival; full suite green.

- [ ] T10 (AC11) Start `api` and wait from `demo`
  - **Test:** `docker compose down`, then `docker compose run --rm demo` - `demo.sh` sends one request and exits 0 only if `api` answered; `docker compose ps api` shows `api` running afterward.
  - **Done when:** red (`no such service: demo`), then green with a `demo` service (`curlimages/curl`, `profiles: ["demo"]`, `depends_on: api`, mounts `demo/demo.sh`) and a retry loop in `demo.sh` that waits for `api` to accept connections; full suite green.

- [ ] T11 (AC12) Send PUT over the compose network
  - **Test:** `docker compose run --rm demo` - `demo.sh` PUTs `http://api:8080/demo/1` and fails unless the status is 200.
  - **Done when:** red (script has no PUT check; point it at a closed port first and see non-zero), then green; full suite green.

- [ ] T12 (AC13) Check the latest body after two PUTs
  - **Test:** `docker compose run --rm demo` - after two PUTs with different bodies, `GET /demo/1` must equal the second body.
  - **Done when:** red (check absent; flip the expected body and see non-zero), then green with the real expected body; full suite green.

- [ ] T13 (AC14) Check the version list and the first version
  - **Test:** `docker compose run --rm demo` - `GET /demo/1/versions` must equal `[1,2]` and `GET /demo/1/versions/1` must equal the first body.
  - **Done when:** red (checks absent; flip an expected value and see non-zero), then green; full suite green.

- [ ] T14 (AC15) Exit non-zero when a check fails
  - **Test:** `docker compose run --rm -e API=http://api:1 demo` exits non-zero. `docker compose down -v` then `docker compose run --rm demo` exits 0.
  - **Done when:** red (exit 0 with failing checks), then green with `set -e` and a `check` helper that prints the failure and exits 1; full suite green.

- [ ] T15 (AC2) (characterization) Start `api` only on `up`
  - **Test:** `docker compose down`, `docker compose up -d`, `docker compose ps --services` prints only `api`.
  - **Done when:** green on arrival (`tests` and `demo` carry `profiles`); full suite green.

- [ ] T16 (AC10) List the exact commands in `README.md`
  - **Test:** run each command in `README.md` as written: `docker compose up --build`, `docker compose run --rm tests`, `docker compose run --rm demo`, `docker compose down`. Each exits 0 (stop `up` after the API logs a listening address).
  - **Done when:** red (`demo verify` fails: out of scope and unimplemented), then green after removing that line; commands match the final compose services; full suite green.
