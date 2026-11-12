---
name: tasks
description: Break an approved spec into an ordered TDD task list at specs/<slug>/tasks.md. Use after a spec is approved and before implementation starts.
---

# Tasks

Write `specs/<slug>/tasks.md` from `specs/<slug>/spec.md`. Do not write code or tests.

## Select spec

- Use the number or slug the user gives (`002` or `002-version-history`).
- Else, if exactly one `approved` spec has no `tasks.md`, use it.
- Else list candidates (number, name, status) and ask.

## Steps

1. Read the spec. If `Status` is not `approved`, ask the user to approve it. On yes, set `Status` to `approved`. On no, stop.
2. Map every acceptance criterion to exactly one task. Add setup tasks only when a test cannot run without them (project scaffold, Docker, compose `tests` and `demo` services).
3. Order tasks so each one builds on a green suite. Walking skeleton first.
4. Write the file using the template below.
5. Append any `[skill:*]`, `[hook:*]`, or `[rule]` notes to `specs/<slug>/scratchpad.md`. Format: see the `work` skill.
6. Show the path. Wait for approval before `work`.

## Template

```markdown
# Tasks: <Feature name>

- [ ] T1 (AC1) <imperative title>
  - **Test:** `<TestClass.TestName>` - what it arrange/acts/asserts.
  - **Done when:** test red for the right reason, then green; full suite green.
```

## Rules

- Keep tasks small: one failing test, one minimal change, one commit.
- Name the test before the code exists.
- Every task has a **Test**. Setup tasks use a smoke test or a Docker command that must exit 0.
- No task for out-of-scope features (see AGENTS.md).
- No task without an AC, except setup.
- Tag a task `(characterization)` when an earlier task's design likely satisfies its AC. Its **Done when** is "green on arrival; full suite green".
