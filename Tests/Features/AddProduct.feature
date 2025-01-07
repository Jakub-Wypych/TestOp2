Feature: Add Product

  Scenario: Adding a product with missing name
    When user submits the form with the Name field missing
    And clicks the Add Product button
    Then the product should not be added
    And the user should see the message "Name is required"

  Scenario: Adding a product with a negative price
    When user submits the form with a negative "Price" (-100)
    And clicks the Add Product button
    Then the product should not be added
    And the user should see the message "Price must be greater than 0"

  Scenario: Adding a product with a negative quantity
    When user submits the form with a negative "Quantity" (-5)
    And clicks the Add Product button
    Then the product should not be added
    And the user should see the message "Quantity cannot be negative"