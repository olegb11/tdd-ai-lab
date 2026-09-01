# 🧪 tdd-ai-lab

> **A Laboratory for Deterministic, AI-Driven TDD Methodology.**  
> *Fast Loop: Red Test -> Green Code -> (Test Fail? -> Rollback) -> Refactor -> Auto-Commit.*
> *Feature Finalization: + Mutation Check (`run-tdd-cycle.cmd --full`, planned gate) before the final Auto-Commit.*

## 🎯 Manifesto & Core Principles

Modern "Vibe Coding" and stateful chat-based AI development inevitably lead to **Loss of Intent**, **Context Drift**, and unmaintainable codebases. 

`tdd-ai-lab` validates a strict, deterministic software engineering methodology where **LLM is not a partner in conversation, but a stateless code compilation unit**.

### Key Rules
1. **Stateless AI Execution:** Zero persistent memory in chat sessions. State is stored solely in the Git repository (Code, Tests, Specs).
2. **Human Owns the Red Phase:** LLMs are strictly forbidden from writing unit tests or business constraints on their own from raw natural language.
3. **Executable Specs First:** Business invariants and domain logic must be formalized in Git before any implementation begins.
4. **Binary Compiler Arbitration:** Code is accepted ONLY if `dotnet test` returns `PASS` (Green). Any error or failing test leads to an immediate `git rollback`.
5. **Human Owns the Refactor Decision:** The LLM may generate refactoring variants, but only the Human decides whether the code has genuinely improved. The automated test suite guarantees that observable behavior has not changed.
6. **Mutation Guard (Feature Finalization Only):** A green suite is necessary but not sufficient - a test can formally pass yet verify nothing (missing `Assert`, wrong condition). The Mutation Agent deliberately mutates the domain code (`+` -> `-`, `>` -> `>=`, `!=` -> `==`, removed calls) and re-runs the suite. A surviving mutant is a bug your tests missed.
7. **Fast Loop vs. Finalization:** The ordinary TDD fast loop is never blocked by mutants. Mutation checking runs only on feature finalization (`run-tdd-cycle.cmd --full`, planned gate): a surviving mutant blocks the **Auto-Commit only** - never the code (no rollback, the implementation is correct).

## 🏗️ 3-Layer Architecture

Knowledge and state are passed between iterations exclusively through **formal artifacts** (C#, Types, Executable Specs), never through ambiguous natural language prompts.

Layer 1: Executable Specs & Invariants (Git / Gherkin .feature)
   |
   +--> (Human Domain Translation)
   |
Layer 2: Red Unit Tests & Rich Domain Types (xUnit/C#)
   |
   +--> (Stateless LLM Payload Trigger)
   |
Layer 3: Minimal Green Implementation (C# Code)

### Layer 1: Executable Specifications (`docs/specs/`)
Human-readable yet rigorous business invariants. Defines expected behaviors, boundary conditions, and domain rules.

### Layer 2: Red Tests & Types (`src/Domain.Tests/`)
Human-written Red tests translated from Layer 1. Enforces **Rich Domain Models** and Value Objects to make invalid system states unrepresentable.

### Layer 3: Minimal Green Code (`src/Domain/`)
LLM generates the absolute minimum C# implementation to pass the current failing test.

## 📂 Repository Structure

```text
tdd-ai-lab/
├── docs/
│   └── specs/
│       ├── discount-policy.feature
│       ├── promo-code-policy.feature
│       └── shopping-cart-policy.feature
├── src/
│   ├── Domain/
│   │   ├── Calculator.cs
│   │   ├── Cart.cs
│   │   ├── Domain.csproj
│   │   ├── Item.cs
│   │   └── PromoCode.cs
│   └── Domain.Tests/
│       ├── CalculatorTests.cs
│       ├── CartTests.cs
│       ├── Domain.Tests.csproj
│       ├── ItemTests.cs
│       └── PromoCodeTests.cs
├── .gitignore
├── README.md
├── run-tdd-cycle.cmd
└── tdd-ai-lab.sln
```

## 🔄 The Development Cycle (Step-by-Step)

### Fast Loop

Formula: Red Test -> Green Code -> (Test Fail? -> Rollback) -> Refactor -> Auto-Commit

The standard TDD cycle. It is **never blocked** by mutants; test failures trigger the standard rollback.

1. **Edit Spec (Human):** Define or update a business rule in `docs/specs/*.feature`.
2. **Write RED Test (Human):** Add a failing test case in `src/Domain.Tests/`. Verify it fails via `dotnet test`.
3. **Generate GREEN Code (LLM):** Implement the minimal C# code in `src/Domain/` required to make the failing test pass.
4. **Arbiter Check:** Execute the local transaction script:
   - `run-tdd-cycle.cmd`
   - If `dotnet test` **FAILS** -> `git reset --hard HEAD` + `git clean -fd src/` (rollback to the last green state).
5. **Refactor:** The LLM generates refactoring variants, but only the Human decides whether the code has genuinely improved. The test suite must stay green and guarantees that observable behavior has not changed.
6. **Auto-Commit:** If `dotnet test` **PASSES**, the script creates an automatic Git commit of the green, refactored state.

### Feature Finalization (`run-tdd-cycle.cmd --full` - planned gate)

1. Complete the Fast Loop until the suite is green (code + refactor).
2. Run `run-tdd-cycle.cmd --full`. The Mutation Agent mutates `src/Domain/` (e.g. flips `+` to `-`, `>` to `>=`, `!=` to `==`, deletes a method call) and re-runs the suite for each mutant.
3. **All mutants killed** -> the script proceeds with the final Auto-Commit.
4. **A mutant survived** -> the script reports: *"Your tests missed a bug: [mutation description]"*:
   - **No `git rollback`** - the implementation is correct, this is not a false red.
   - The Auto-Commit is **blocked**.
   - The **Human** writes an additional Red test closing the blind spot (Human Owns the Red Phase); then the Fast Loop resumes.

## 🧬 Mutation Agent

In ordinary TDD a test can pass (Green) yet verify nothing - for example, a forgotten `Assert` or a wrong condition. The Mutation Agent closes that gap by deliberately corrupting the domain code:

- `+` -> `-`
- `>` -> `>=`
- `a != b` -> `a == b`
- `if (code.Length != 6)` -> `if (code.Length == 6)`
- removed method calls

For each mutant the agent re-runs the test suite:

- **Killed:** at least one test fails -> the suite genuinely guards this behavior.
- **Survived:** all tests still pass -> the suite is blind to this behavior -> the agent reports to the Human: *"Your tests missed a bug: [mutation description]"*.

The Mutation Agent runs on feature finalization only (`run-tdd-cycle.cmd --full`, planned gate), never inside the fast loop. It never rolls back the implementation - it guards the **tests**, not the code.

## 🛠️ Stack
* **Language:** C# / .NET 8+
* **Testing Framework:** xUnit (Assert)
* **AI Engine:** Stateless API Payload (Claude / OpenAI / Local LLM)
* **Control:** Windows CMD (run-tdd-cycle.cmd) / Git CLI

## 📜 License
MIT
