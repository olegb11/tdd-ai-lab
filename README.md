# Shopping Cart Domain (TDD Engine)

A robust, specification-driven C# domain model for a Shopping Cart system, built using strict **Test-Driven Development (TDD)** practices and **Executable Specifications (Gherkin)**.

---

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

---

## 🏗 System Architecture

The project follows a **3-Layer Living Documentation** approach:

1. **Layer 1: Executable Specifications (`docs/specs/*.feature`)**
   - Formalized using standard Gherkin syntax (`Feature`, `Scenario Outline`, `Given-When-Then`).
   - Serves as the single source of truth for business invariants.
2. **Layer 2: Test Suite (`src/Domain.Tests/`)**
   - Unit tests implemented with **xUnit** (`[Fact]`, `[Theory]`).
   - Verifies all scenarios defined in Gherkin specs.
3. **Layer 3: Domain Model (`src/Domain/`)**
   - Clean C# domain primitives (`Item`, `PromoCode`, `Cart`) enforcing strict validation and business rules.

---

## ⚙️ Tech Stack & Tooling

- **Language**: C# / .NET 8
- **Testing Framework**: xUnit (`Assert`)
- **Specification Format**: Gherkin (`docs/specs/*.feature`)
- **Automation Engine**: Custom `run-tdd-cycle.cmd` script for automated Red-Green-Rollback flow.

---

## 📋 Core Business Rules

### 1. Item Policy (`docs/specs/discount-policy.feature`)
- **Price**: Must be non-negative (`Price >= 0`).
- **Quantity**: Must be at least 1 (`Quantity >= 1`).
- **Discount**: Item-level discount range is `0% - 100%`.
- **Item Total Formula**:
  $$\text{ItemTotal} = \text{Price} \times \text{Quantity} \times \left(1 - \frac{\text{Discount}}{100}\right)$$

### 2. Promo Code Policy (`docs/specs/promo-code-policy.feature`)
- **Format**: Must be non-empty and **exactly 6 characters** long (e.g., `SAVE10`).
- **Discount Percentage**: Range is `1% - 100%`.
- **Expiration**: Checked against current evaluation date (`IsValid(currentDate)`).

### 3. Shopping Cart Policy (`docs/specs/shopping-cart-policy.feature`)
- Empty cart calculates total as `0.00`.
- Promo codes apply discount to the subtotal of items.
- Applying an expired or invalid promo code throws an `InvalidOperationException`.

---

## 🔄 TDD Automation Lifecycle (`run-tdd-cycle.cmd`)

The project uses an automated TDD controller script to enforce code quality and Git discipline:

1. **Execution**: Compiles code and runs unit tests via `dotnet test`.
2. **On GREEN State**: Automatically stages modified files in `src/` and `docs/specs/`, committing them with an English commit message.
3. **On RED State**: Automatically rolls back repository changes (`git reset --hard HEAD` and `git clean -fd src/`) to guarantee the main branch never stays in a broken state.

### Running the TDD Engine
Execute in your terminal or Far Manager:

```cmd
run-tdd-cycle.cmd
```