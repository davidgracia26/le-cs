# Tasks: Save and fetch documents

- [x] S1 (setup) Run tests in Docker
  - **Test:** `SmokeTests.Factory_Starts_Host` - build `WebApplicationFactory<Program>` with a temp `DATA_DIR`, create a client, assert the factory builds.
  - **Done when:** `docker compose run --rm tests` exits 0. Covers: `Microsoft.AspNetCore.Mvc.Testing` added, `coverlet.collector` dropped, `UnitTest1.cs` deleted, `public partial class Program { }`, `Dockerfile` with an sdk `tests` target, `.dockerignore`, `tests` service under a compose `profiles` entry. Red first: command fails (no Dockerfile/compose). Then green.

- [x] T1 (AC25) Remove template leftovers
  - **Test:** `LeftoverTests.WeatherForecast_Returns404` - GET `/weatherforecast/1`, assert 404.
  - **Done when:** red (200 from template), then green after deleting `/weatherforecast`, `WeatherForecast`, `AddOpenApi`, `MapOpenApi`, `UseHttpsRedirection`, the OpenApi package, and `LE.CS.Api.http`; suite green.

- [x] T2 (AC1) Save a document with PUT
  - **Test:** `SaveTests.Put_Returns_Version_1` - PUT `/books/1` `{"title":"Dune"}`, assert 200 and body `{"version":1}`.
  - **Done when:** red (404), then green with `IDocumentStore` + JSONL impl via DI (`DATA_DIR`, default `/data`); suite green.

- [x] T3 (AC2) Fetch the latest document
  - **Test:** `FetchTests.Get_Returns_Saved_Document` - PUT then GET `/books/1`, assert 200, `Content-Type: application/json`, body `{"title":"Dune"}`.
  - **Done when:** red (404), then green; suite green.

- [x] T4 (AC3) Persist one compact line per document file
  - **Test:** `StorageTests.Put_Writes_Jsonl_File` - PUT with temp `DATA_DIR`, assert `books/1.jsonl` exists with exactly one line `{"title":"Dune"}`.
  - **Done when:** red for the right reason (missing file or wrong line), then green; suite green.

- [x] T5 (AC4) Accept non-object bodies
  - **Test:** `SaveTests.Put_NonObject_Roundtrips` - `[Theory]` over `[1,2]`, `"text"`, `42`, `null`: PUT asserts 200, GET asserts the same JSON.
  - **Done when:** red per case, then green; suite green.

- [x] T6 (AC5) Keep text without added escapes
  - **Test:** `FetchTests.Get_Keeps_Unicode_Unescaped` - PUT `{"s":"é<&>日本"}`, GET, assert identical text and no `\u` in body.
  - **Done when:** red (escapes added), then green with `UnsafeRelaxedJsonEscaping`; suite green.

- [x] T7 (AC6) Keep key order
  - **Test:** `FetchTests.Get_Keeps_Key_Order` - PUT `{"b":1,"a":2}`, GET, assert body `{"b":1,"a":2}`.
  - **Done when:** red, then green by writing `JsonElement` values; suite green.

- [x] T8 (AC7) Accept `text/plain`
  - **Test:** `SaveTests.Put_TextPlain_Returns200` - PUT with `Content-Type: text/plain`, assert 200.
  - **Done when:** red (415 or 400), then green by reading the raw body; suite green.

- [x] T9 (AC8) Accept no `Content-Type`
  - **Test:** `SaveTests.Put_NoContentType_Returns200` - PUT with `Content-Type` header absent, assert 200.
  - **Done when:** red, then green; suite green.

- [x] T10 (AC9) Return 404 for a missing document
  - **Test:** `FetchTests.Get_Missing_Returns404` - GET `/books/missing`, assert 404.
  - **Done when:** red, then green; suite green.

- [x] T11 (AC10) List ids in a collection
  - **Test:** `ListTests.List_Returns_Sorted_Ids` - PUT `books/2` then `books/1`, GET `/books`, assert 200, `Content-Type: application/json`, body `["1","2"]`.
  - **Done when:** red (404), then green by listing files, ordinal sort; suite green.

- [x] T12 (AC11) List an empty collection
  - **Test:** `ListTests.List_Empty_Collection_Returns_EmptyArray` - GET `/nothing`, assert 200 and body `[]`.
  - **Done when:** red (exception or 404), then green; suite green.

- [x] T13 (AC12) Reject invalid JSON
  - **Test:** `ValidationTests.Put_InvalidJson_Returns400` - PUT `/books/1` body `{bad`, assert 400.
  - **Done when:** red (500), then green; suite green.

- [ ] T14 (AC13) Reject an empty body
  - **Test:** `ValidationTests.Put_EmptyBody_Returns400` - PUT `/books/1` with empty body, assert 400.
  - **Done when:** red, then green; suite green.

- [ ] T15 (AC14) Reject a bad collection on PUT
  - **Test:** `ValidationTests.Put_BadCollection_Returns400` - PUT `/bad.name/1`, assert 400.
  - **Done when:** red, then green with a `[A-Za-z0-9_-]{1,100}` check; suite green.

- [ ] T16 (AC15) Reject a bad id on PUT
  - **Test:** `ValidationTests.Put_BadId_Returns400` - PUT `/books/bad.id`, assert 400.
  - **Done when:** red, then green; suite green.

- [ ] T17 (AC16) Reject a bad collection on list
  - **Test:** `ValidationTests.List_BadCollection_Returns400` - GET `/bad.name`, assert 400.
  - **Done when:** red, then green; suite green.

- [ ] T18 (AC17) Reject a bad collection on fetch
  - **Test:** `ValidationTests.Get_BadCollection_Returns400` - GET `/bad.name/1`, assert 400.
  - **Done when:** red, then green; suite green.

- [ ] T19 (AC18) Accept letters, digits, `_`, `-`
  - **Test:** `ValidationTests.Put_ValidChars_Returns200` - PUT `/Books_2/doc-1`, assert 200.
  - **Done when:** test green on arrival is allowed only if T15 already allows it; else red then green; suite green.

- [ ] T20 (AC19) Accept a 100-character id
  - **Test:** `ValidationTests.Put_Id100_Returns200` - PUT `/books/{100 x 'a'}`, assert 200.
  - **Done when:** red or already-green noted; suite green.

- [ ] T21 (AC20) Reject a 101-character id
  - **Test:** `ValidationTests.Put_Id101_Returns400` - PUT `/books/{101 x 'a'}`, assert 400.
  - **Done when:** red, then green; suite green.

- [ ] T22 (AC21) Accept a 100-character collection
  - **Test:** `ValidationTests.Put_Collection100_Returns200` - PUT `/{100 x 'a'}/1`, assert 200.
  - **Done when:** red or already-green noted; suite green.

- [ ] T23 (AC22) Reject a 101-character collection
  - **Test:** `ValidationTests.Put_Collection101_Returns400` - PUT `/{101 x 'a'}/1`, assert 400.
  - **Done when:** red, then green; suite green.

- [ ] T24 (AC23) Save with POST
  - **Test:** `SaveTests.Post_Returns_Version_1` - POST `/books/1` `{"title":"Dune"}`, assert 200 and body `{"version":1}`.
  - **Done when:** red (405), then green by mapping POST to the PUT handler; suite green.

- [ ] T25 (AC24) Fetch after POST
  - **Test:** `FetchTests.Get_After_Post_Returns_Document` - POST then GET `/books/1`, assert 200 and body `{"title":"Dune"}`.
  - **Done when:** red or already-green noted; suite green.
