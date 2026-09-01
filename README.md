# 🧪 tdd-ai-lab

> **A Laboratory for Deterministic, AI-Driven TDD Methodology.**  
> *Red Test -> Green Code -> (Test Fail? -> Rollback) -> Refactor -> Auto-Commit.*

## 🎯 Manifesto & Core Principles

Modern "Vibe Coding" and stateful chat-based AI development inevitably lead to **Loss of Intent**, **Context Drift**, and unmaintainable codebases. 

`tdd-ai-lab` validates a strict, deterministic software engineering methodology where **LLM is not a partner in conversation, but a stateless code compilation unit**.

### Key Rules
1. **Stateless AI Execution:** Zero persistent memory in chat sessions. State is stored solely in the Git repository (Code, Tests, Specs).
2. **Human Owns the Red Phase:** LLMs are strictly forbidden from writing unit tests or business constraints on their own from raw natural language.
3. **Executable Specs First:** Business invariants and domain logic must be formalized in Git before any implementation begins.
4. **Binary Compiler Arbitration:** Code is accepted ONLY if `dotnet test` returns `PASS` (Green). Any error or failing test leads to an immediate `git rollback`.
5. **Human Owns the Refactor Decision:** The LLM may generate refactoring variants, but only the Human decides whether the code has genuinely improved. The automated test suite guarantees that observable behavior has not changed.

## 🏗️ 3-Layer Architecture

Knowledge and state are passed between iterations exclusively through **formal artifacts** (C#, Types, Executable Specs), never through ambiguous natural language prompts.

Layer 1: Executable Specs & Invariants (Git / Markdown)
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

Red Test -> Green Code -> (Test Fail? -> Rollback) -> Refactor -> Auto-Commit

1. **Edit Spec (Human):** Define or update a business rule in `docs/specs/*.feature`.
2. **Write RED Test (Human):** Add a failing test case in `src/Domain.Tests/`. Verify it fails via `dotnet test`.
3. **Generate GREEN Code (LLM):** Implement the minimal C# code in `src/Domain/` required to make the failing test pass.
4. **Arbiter Check:** Execute the local transaction script:
   - `run-tdd-cycle.cmd`
   - If `dotnet test` **FAILS** -> `git reset --hard HEAD` + `git clean -fd src/` (rollback to the last green state).
5. **Refactor:** The LLM generates refactoring variants, but only the Human decides whether the code has genuinely improved. The test suite must stay green and guarantees that observable behavior has not changed.
6. **Auto-Commit:** If `dotnet test` **PASSES**, the script creates an automatic Git commit of the green, refactored state.

## 🛠️ Stack
* **Language:** C# / .NET 8+
* **Testing Framework:** xUnit, FluentAssertions
* **AI Engine:** Stateless API Payload (Claude / OpenAI / Local LLM)
* **Control:** Bash / Git CLI Hooks

## 📜 License
MIT
