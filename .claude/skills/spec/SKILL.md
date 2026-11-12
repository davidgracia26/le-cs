---
name: spec
description: Write a feature spec at specs/<slug>/spec.md with numbered, testable acceptance criteria. Use when the user wants to define, plan, or specify a feature or change before building it.
---

# Spec

Write `specs/<slug>/spec.md`. Do not write code or tests.

## Slug

`<slug>` = `NNN-kebab-name`. `NNN` = highest existing number in `specs/` plus 1, zero-padded to 3 digits (`001`, `002`). Example: `specs/002-version-history/spec.md`. Never reuse or renumber.

## Steps

1. Read `AGENTS.md`, `GUIDANCE.md`, `briefs/`, and existing `specs/`. Do not duplicate or contradict an existing spec; reference it by number instead.
2. Ask the user at most 3 questions, only for gaps that change behavior. Skip if none.
3. Reject or flag any item on the AGENTS.md "Out of scope" list. Do not write it into the spec.
4. Write the spec using the template below. Keep it under one page. Check every AC against the others for contradictions.
5. Create `specs/<slug>/scratchpad.md` with the heading `# Scratchpad`. This opens the cycle's notes. Format and use: see the `work` skill.
6. Show the path. Wait for approval before `tasks`.

## Template

```markdown
# <Feature name>

**Status:** draft | approved

## Goal
One sentence.

## Behavior
Numbered acceptance criteria (AC1, AC2, ...). Each one:
- States one observable result: request/input -> status code and body, or file effect.
- Has no "and". Split instead.
- Can fail an automated test.

## Non-goals
Items deliberately left out.
```

## Rules

- Write observable behavior only. Name no classes, files, or libraries unless AGENTS.md fixes them.
- Cover error paths (400, 404) as their own criteria.
- Mark `Status: approved` only after the user says so.
