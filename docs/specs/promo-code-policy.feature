Feature: Promo Code Domain Validation
  In order to ensure correct promotional discount application
  As a Shopping Cart domain
  I want to validate promo code format, discount bounds, and expiration status

  Scenario Outline: Reject creation of promo code with invalid parameters
    When I try to create a promo code with code "<code>", discount <discount>, and no expiration
    Then the system should reject it with the error "<error_message>"

    Examples:
      | code   | discount | error_message                    |
      |        | 10       | Promo code cannot be empty       |
      |        | 10       | Promo code cannot be empty       |
      | SAVE10 | 0        | Promo discount must be at least 1% |
      | SAVE10 | 101      | Promo discount cannot exceed 100% |

  Scenario: Expired promo code is invalid
    Given a promo code with expiration date in the past
    When I check its validity for the current date
    Then the system should return false

  Scenario: Active non-expired promo code is valid
    Given a promo code with expiration date in the future
    When I check its validity for the current date
    Then the system should return true