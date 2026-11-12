# Tasks: Version history

- [x] T1 (AC1) Return version 2 on the second PUT
  - **Test:** `VersionTests.Second_Put_Returns_Version_2` - PUT `/books/1` `{"version":1}` then `{"title":"Other"}`, assert second response 200 and body `{"version":2}`.
  - **Done when:** red (body `{"version":1}`), then green by returning the line count from `Save`; full suite green.

- [x] T2 (AC2) (characterization) Fetch the latest after two PUTs
  - **Test:** `VersionTests.Get_Returns_Second_Body` - two PUTs with different bodies, GET `/books/1`, assert body equals the second.
  - **Done when:** green on arrival; full suite green.

- [x] T3 (AC3) List version numbers
  - **Test:** `VersionTests.Versions_Lists_1_2_3` - three PUTs, GET `/books/1/versions`, assert 200, `Content-Type: application/json`, body `[1,2,3]`.
  - **Done when:** red (404), then green with `GET /{collection}/{id}/versions`; full suite green.

- [x] T4 (AC4) Fetch version 1
  - **Test:** `VersionTests.Version_1_Returns_First_Body` - two PUTs, GET `/books/1/versions/1`, assert 200, `Content-Type: application/json`, body equals the first PUT body.
  - **Done when:** red (404), then green with `GET /{collection}/{id}/versions/{n}`; full suite green.

- [x] T5 (AC5) (characterization) Fetch version 2
  - **Test:** `VersionTests.Version_2_Returns_Second_Body` - two PUTs, GET `/books/1/versions/2`, assert 200 and body equals the second PUT body.
  - **Done when:** green on arrival; full suite green.

- [x] T6 (AC6) (characterization) Version on POST
  - **Test:** `VersionTests.Post_On_Existing_Returns_Version_2` - PUT `/books/1`, POST `/books/1`, assert POST 200 and body `{"version":2}`.
  - **Done when:** green on arrival; full suite green.

- [x] T7 (AC7) (characterization) Keep identical bodies as new versions
  - **Test:** `VersionTests.Identical_Put_Adds_Version` - PUT the same body twice, assert second response 200 and `GET /books/1/versions` is `[1,2]`.
  - **Done when:** green on arrival; full suite green.

- [ ] T8 (AC8) (characterization) Return 404 above the last version
  - **Test:** `VersionTests.Version_Beyond_Last_Returns404` - three PUTs, GET `/books/1/versions/9`, assert 404.
  - **Done when:** green on arrival; full suite green.

- [ ] T9 (AC9) Return 404 for version 0
  - **Test:** `VersionTests.Version_0_Returns404` - PUT `/books/1`, GET `/books/1/versions/0`, assert 404.
  - **Done when:** red (exception or wrong status), then green with the `^[1-9][0-9]*$` check; full suite green.

- [ ] T10 (AC10) Return 404 for a non-numeric version
  - **Test:** `VersionTests.Version_Abc_Returns404` - PUT `/books/1`, GET `/books/1/versions/abc`, assert 404.
  - **Done when:** red or already-green noted; full suite green.

- [ ] T11 (AC11) Return 404 for versions of a missing document
  - **Test:** `VersionTests.Versions_Of_Missing_Returns404` - GET `/books/missing/versions`, assert 404.
  - **Done when:** red (200 `[]` or exception), then green; full suite green.

- [ ] T12 (AC12) Return 404 for a version of a missing document
  - **Test:** `VersionTests.Version_Of_Missing_Returns404` - GET `/books/missing/versions/1`, assert 404.
  - **Done when:** red (exception) or already-green noted; full suite green.

- [ ] T13 (AC13) Reject a bad collection on versions
  - **Test:** `ValidationTests.Versions_BadCollection_Returns400` - GET `/bad.name/1/versions`, assert 400.
  - **Done when:** red (404), then green; full suite green.

- [ ] T14 (AC14) Reject a bad id on one version
  - **Test:** `ValidationTests.Version_BadId_Returns400` - GET `/books/bad.id/versions/1`, assert 400.
  - **Done when:** red (404), then green with name checks before the version check; full suite green.

- [ ] T15 (AC15) Isolate documents
  - **Test:** `VersionTests.Put_Leaves_Other_Documents_Unchanged` - PUT `/books/1` twice and `/books/2` once, assert `/books/1/versions` is `[1,2]` and `/books/2/versions` is `[1]`.
  - **Done when:** red or already-green noted; full suite green.

- [ ] T16 (AC16) (characterization) Add no version on a failed PUT
  - **Test:** `VersionTests.Failed_Put_Adds_No_Version` - PUT valid body, PUT `{bad` (assert 400), assert `/books/1/versions` is `[1]`.
  - **Done when:** green on arrival; full suite green.

- [ ] T17 (AC17) Write n lines ending with a newline
  - **Test:** `StorageTests.File_Has_One_Line_Per_Version` - temp `DATA_DIR`, three PUTs, read `books/1.jsonl`, assert exactly 3 lines and text ends with `\n`.
  - **Done when:** red or already-green noted; full suite green.

- [ ] T18 (AC18) Serialize concurrent PUTs
  - **Test:** `ConcurrencyTests.Concurrent_Puts_Keep_All_Versions` - 50 parallel PUTs of distinct bodies to `/books/1`, assert all 200, `/books/1/versions` is `[1..50]`, each version parses as JSON and equals a sent body, no body repeats.
  - **Done when:** red (duplicate version numbers or interleaved lines), then green with a per-document `SemaphoreSlim` around append and count; full suite green.
