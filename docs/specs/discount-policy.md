# Discount Policy Specification

## Business Rules
1. An empty shopping cart has a total price of 0.
2. Item total price is calculated as `Price * Quantity`.
3. If a percentage discount is specified, it reduces the item total price accordingly: `ItemTotal - (ItemTotal * (Discount / 100))`.

## Invariants & Guardrails
- Price cannot be negative (< 0).
- Quantity must be a positive integer (>= 1).
- Discount percentage must be between 0 and 100 inclusive (0..100).
