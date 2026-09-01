# 🧪 tdd-ai-lab

> **A Laboratory for Deterministic, AI-Driven TDD Methodology.**  
> *Executable Specs -> Red Tests (Human) -> Green Code (LLM) -> Automatic Rollback.*

## 🎯 Manifesto & Core Principles

Modern "Vibe Coding" and stateful chat-based AI development inevitably lead to **Loss of Intent**, **Context Drift**, and unmaintainable codebases. 

`tdd-ai-lab` validates a strict, deterministic software engineering methodology where **LLM is not a partner in conversation, but a stateless code compilation unit**.

### Key Rules
1. **Stateless AI Execution:** Zero persistent memory in chat sessions. State is stored solely in the Git repository (Code, Tests, Specs).
2. **Human Owns the Red Phase:** LLMs are strictly forbidden from writing unit tests or business constraints on their own from raw natural language.
3. **Executable Specs First:** Business invariants and domain logic must be formalized in Git before any implementation begins.
4. **Binary Compiler Arbitration:** Code is accepted ONLY if `dotnet test` returns `PASS` (Green). Any error or failing test leads to an immediate `git rollback`.

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

[1. Edit Spec] -> [2. Write Red Test] -> [3. Run tdd-step.sh] -> [4. Commit / Rollback]

1. **Formalize Requirement:** Define or update a business rule in `docs/specs/*.md`.
2. **Write RED Test (Human):** Add a failing test case in `src/Domain.Tests/`. Verify failure via `dotnet test`.
3. **Trigger AI Generation:** Run the local transaction CLI:
   ./tdd-step.sh "Implement percentage discount calculation"
4. **Arbitration & Transaction:**
   * If `dotnet test` **PASSES** -> Automatic Git Commit.
   * If `dotnet test` **FAILS** -> Immediate `git checkout` (Rollback to clean state).

## 🛠️ Stack
* **Language:** C# / .NET 8+
* **Testing Framework:** xUnit, FluentAssertions
* **AI Engine:** Stateless API Payload (Claude / OpenAI / Local LLM)
* **Control:** Bash / Git CLI Hooks

## 📜 License
MIT
