# Tasks: Demo rerun

- [x] T1 (AC1) (characterization) Run the demo on a fresh volume
  - **Test:** `docker compose down -v`, then `docker compose run --rm demo` exits 0.
  - **Done when:** green on arrival (exit 0); full suite green.

- [x] T2 (AC2) Rerun the demo on the same volume
  - **Test:** after T1's run, `docker compose run --rm demo` again exits 0, with no `down` between runs.
  - **Done when:** red (non-zero: `FAIL GET versions: expected '[1,2]' got '[1,2,3,4]'`), then green with `demo.sh` saving under a run-unique id instead of `demo/1`; full suite green.

- [ ] T3 (AC3) (characterization) Rerun the demo a third time
  - **Test:** a third consecutive `docker compose run --rm demo` exits 0.
  - **Done when:** green on arrival (exit 0); full suite green.

- [ ] T4 (AC4) (characterization) Use a new id each run
  - **Test:** `docker compose down -v`, run `docker compose run --rm demo` twice, then `docker run --rm --network le-cs_default curlimages/curl -s http://api:8080/demo` lists two ids, and the lists after run 1 and run 2 differ.
  - **Done when:** green on arrival (two distinct ids listed); full suite green.

- [ ] T5 (AC5) (characterization) Expect exactly `[1,2]` on every run
  - **Test:** read `demo/demo.sh`: the `GET versions` check still compares to the literal `[1,2]`. Run the demo twice more; both exit 0.
  - **Done when:** green on arrival (literal unchanged, exit 0 on both runs); full suite green.

- [ ] T6 (AC6) (characterization) Leave earlier documents unchanged
  - **Test:** after one demo run, record `GET /demo/{id}/versions` for every id in `GET /demo` (curl via `le-cs_default`). Run the demo again, then repeat the requests for the recorded ids. Outputs match.
  - **Done when:** green on arrival (identical output for each recorded id); full suite green.

- [ ] T7 (AC7) (characterization) Still fail on a failed check
  - **Test:** `docker compose run --rm -e API=http://api:1 demo` exits non-zero.
  - **Done when:** green on arrival (non-zero); full suite green.

- [ ] T8 (AC8) (characterization) Keep the README demo command
  - **Test:** read `README.md`: the demo command is exactly `docker compose run --rm demo`.
  - **Done when:** green on arrival; `docker compose down`; full suite green.
