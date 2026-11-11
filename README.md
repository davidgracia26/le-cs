# Pre-interview exercise

Thanks for making the time. Before we talk, we'd like you to spend **one hour** building
something small. We'll then spend most of the live session with you walking us through it.

Pick **one** of these three. They're all about the same size — pick the one you'd rather
talk about for an hour.

| Brief | In one line |
| --- | --- |
| [Document store](briefs/document-store.md) | Store and retrieve JSON documents, keeping previous versions. |

---

## The rules

**1. Any language, framework, datastore, or runtime.** Genuinely any. Pick what you'd
actually reach for. We work across a lot of technologies and we are not checking whether
you write a particular one the way we would.

**2. It has to be containerized, and Docker is the only thing you can assume.** Assume the
machine we run it on has Docker (with Compose) and nothing else: no language runtime, no
package manager, no `make`, no `curl`, no particular shell. Everything your project needs,
toolchain included, runs inside containers. One command should bring it up.

**3. It has to be shown to work.** Automated tests, a demo script that exercises it, sample
runs — we don't care which. We care that you picked one and can say why it was the right
one for this. Whatever you choose goes in the repo, and we have to be able to run it with
Docker alone (for example, a compose service that runs the tests or the demo). Screenshots
or a short recording are welcome as extras, not as the only proof. You'll also run it live
for us in the session.

**4. Tell us how to run it.** A few lines in your README with the exact commands to start
it and to run your proof. Not a document — just enough that we can start it.

**5. Commit the way you normally would.** How the work unfolded is part of what we read.

**6. Build it fresh.** Not something you already had lying around.

**7. One hour.** We've scoped these to fit in an hour, container included. **A polished
three-hour submission reads worse to us than a clean one-hour one** — scoping is part of
what we're looking at. If you finish early, stop.

**8. AI tools are welcome, and so is not using them.** Use your normal setup. We'll ask you
about it either way, and "I didn't, because..." is a perfectly good answer.

**9. Beyond those run instructions, there's nothing to write up.** No design doc, no trade-offs memo, no summary. We'd
much rather hear it from you in the session than read a tidied-up version beforehand. Jot
notes for yourself if it helps you present — we won't ask to see them.

**10. This is not pass/fail.** Your interview is booked and it's happening regardless of
what you submit. We don't reject anyone on this exercise.

---

## What we'll do with it

Send us a link to your repo **at least 48 hours before the session** so we can read it
properly.

The live session is 90 minutes and most of it is your project:

- **~20 minutes** — you share your screen, run it, and walk us through what you built.
- **~40 minutes** — we go through it with you the way we'd go through a teammate's pull
  request. Expect questions about why you chose what you chose, what it cost you, and what
  you'd do differently.
- **~25 minutes** — explaining a technical decision to a non-technical audience, and then
  your questions for us.

The thing we're most interested in is your reasoning. Every choice in here has a trade-off;
we want to hear you name them.

## If something gets in your way

Email us. Getting stuck on a broken toolchain isn't what we're trying to measure, and
asking costs you nothing.
