# Tasks: Post only

- [x] T1 (AC4) (characterization) Save with POST in every test
  - **Test:** replace `Helpers.PutJson` with `PostJson` (`client.PostAsync`). Change every `PutJson` and `PutAsync` call in `LE.CS.Tests` to POST and rename `Put_*` tests to `Post_*`. Where a renamed test now duplicates an existing POST test (the `PostAsync` tests in `SaveTests`, `VersionTests`, `FetchTests`), keep one copy.
  - **Done when:** green on arrival (POST already saves); a case-sensitive search for `Put` in `LE.CS.Tests` finds nothing (T2 and T3 add the only `Put` uses later); full suite green.

- [ ] T2 (AC1) Reject PUT on a new document
  - **Test:** `PutRemovedTests.Put_Returns405` - `PUT /books/1` with `{"title":"Dune"}` -> 405. Use `HttpClient.PutAsync` directly, not a helper.
  - **Done when:** red (200), then green with the save route mapped to `POST` only in `Program.cs`; full suite green.

- [ ] T3 (AC2) (characterization) Reject PUT on an existing document
  - **Test:** `PutRemovedTests.Put_On_Existing_Returns405_And_Adds_No_Version` - POST `/books/1` twice, `PUT /books/1` -> 405, `GET /books/1/versions` is `[1,2]`.
  - **Done when:** green on arrival; full suite green.

- [ ] T4 (AC3) (characterization) Keep POST behavior
  - **Test:** existing `SaveTests.Post_Returns_Version_1` and `VersionTests.Second_Post_Returns_Version_2` cover this; add no test.
  - **Done when:** both tests green by name in `docker compose run --rm tests`; full suite green.

- [ ] T5 (AC6) Use POST in the demo
  - **Test:** `docker compose down -v`, then `docker compose run --rm demo` exits 0.
  - **Done when:** red (non-zero: `demo.sh` sends `PUT`, now 405), then green with `demo.sh` using `-X POST` and `POST first` / `POST second` check labels; `docker compose down -v`; full suite green.

- [ ] T6 (AC7) Remove PUT from the docs
  - **Test:** search `AGENTS.md` and `README.md` for `PUT` (case-sensitive). Result must be empty.
  - **Done when:** red (4 matches in `AGENTS.md`), then green with the API, storage, and content-type lines naming `POST` only; full suite green.

- [ ] T7 (AC5) (characterization) Run the suite through compose
  - **Test:** `docker compose run --rm tests` exits 0.
  - **Done when:** green on arrival (exit 0); full suite green.
