---
name: work
description: Implement the next unchecked task in specs/<slug>/tasks.md using strict red-green-refactor TDD, run through Docker, one commit per task. Use when the user wants to start or continue building from a task list.
---

# Work

Complete tasks in order. One task at a time. Use `todo_write` to track.

## Select spec

- Use the number or slug the user gives (`002` or `002-version-history`).
- Else, if exactly one `specs/*/tasks.md` has unchecked tasks, use it.
- Else list candidates (number, name, open/total tasks) and ask.
- Touch only the selected spec's `tasks.md`. Prefix commit messages with the spec number (`002: ...`).

## Loop (per task)

1. **Pick:** first unchecked task in `tasks.md`. Re-read its AC in `spec.md`.
2. **Red:** write the named test only. Run `docker compose run --rm tests`. Confirm it fails for the expected reason (assertion, not compile or setup error). If it passes because earlier code already satisfies the AC, keep it as a characterization test, say so in the commit message, and continue. Stop and tell the user only if the test cannot fail.
3. **Green:** write the least code that passes. No extra features, no speculative abstractions.
4. **Refactor:** clean up with the suite green. Match AGENTS.md conventions.
5. **Verify:** run the full suite. All tests pass.
6. **Close:** check the box in `tasks.md`. Commit code, test, and `tasks.md` together. Message states the AC and the why.
7. **Next:** repeat. Stop when all boxes are checked.

## Scratchpad

Append to `specs/<slug>/scratchpad.md` (created by `spec`) throughout the cycle. One line per note, tagged:
- `[skill:<name>]` a skill step was unclear, missing, wrong, or wasteful.
- `[hook:<event>]` a command or check you ran or should have run repeatedly or manually (e.g. run tests after edit, block `bin/`/`obj/` commits, block local `dotnet`). Name the trigger and the command.
- `[rule]` a convention that belongs in `AGENTS.md`.

Write the note when you hit it. Do not batch.

## Retrospective (after the last task, before reporting)

1. Read the scratchpad with `cat specs/<slug>/scratchpad.md`. It is gitignored, so glob and search tools miss it. Drop duplicates and one-offs. Keep only notes that would change future runs.
2. Update or create skills in `.claude/skills/<name>/SKILL.md` for `[skill:*]` notes. Keep edits minimal. Edit existing skills before adding new ones.
3. Update or create hooks for `[hook:*]` notes. Put scripts in `.claude/hooks/`. Register them in `.claude/settings.json` (create it if absent; merge, never overwrite). Use a script that runs on the user's platform; no new dependencies. Run each new hook once to confirm it works.
4. Add `[rule]` notes to `AGENTS.md`.
5. Delete `specs/<slug>/scratchpad.md`. Delete only after steps 2 to 4 finish.
6. Commit the skill, hook, settings, and `AGENTS.md` edits as the spec's final commit, prefixed with the spec number (`002: retrospective ...`). One spec cycle reads as one change in history.
7. Report: list each file created or updated with one line on why. State "none" if nothing qualified.

## Rules

- Never write production code without a failing test first.
- Never edit a test to make it pass. Change a test only if the spec changed; update the spec first and tell the user.
- Run everything through Docker only. No local `dotnet`.
- Stop and ask on: ambiguous AC, a needed out-of-scope feature, or a failing test you cannot explain after two attempts.
- Do not skip, comment out, or delete failing tests.
- When all tasks are done, run AGENTS.md "Verify before finishing". Report each item pass or fail.
