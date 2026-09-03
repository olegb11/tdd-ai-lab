# Instructions for AI Agents (`tdd-ai-lab`)

## 1. Single Source of Truth
All architectural standards, tech stack choices (.NET 8, React), and project rules are strictly defined in:
- **`README.md`** — Core domain architecture, technology stack, and engineering standards.
- **`/docs/specs/*.feature`** — Mandatory BDD business specifications (Gherkin).

## 2. Agent Execution Guardrails
- **Strict TDD First**: Follow the Red-Green-Refactor cycle. Never implement production code without a corresponding failing test.
- **No "Vibe-Coding"**: Do not speculate or invent domain logic. Rely strictly on `/docs/specs/*.feature` and `README.md`.
- **Atomic Execution**: Make clean, incremental changes and ensure all unit and integration tests pass.
