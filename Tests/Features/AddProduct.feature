Feature: Add product

  Scenario: Adding a product via the form
    Given the user is on the "Add Product" page
    When the user fills in the form with valid data ("Name": "Smartphone", "Date": "2025-01-01", "Price": 1000, "Category": "Electronics", "Quantity": 50)
    And clicks the "Add Product" button
    Then the product "Smartphone" with price 1000 should be added to the list of products
    And the form should be cleared and a success message should appear

   Scenario: Adding a product with missing name
    Given the user is on the "Add Product" page
    When the user submits the form with missing "Name" field
    Then the form should not be submitted
    And the user should see the message "Name is required"

  Scenario: Adding a product with negative price
    Given the user is on the "Add Product" page
    When the user submits the form with a negative "Price" (-100)
    Then the form should not be submitted
    And the user should see the message "Price must be greater than 0"

  Scenario: Adding a product with invalid quantity (negative)
    Given the user is on the "Add Product" page
    When the user submits the form with a negative "Quantity" (-5)
    Then the form should not be submitted
    And the user should see the message "Quantity cannot be negative"

  Scenario: Adding a product with an empty price
    Given the user is on the "Add Product" page
    When the user submits the form with an empty "Price" field
    Then the form should not be submitted
    And the user should see the message "Price is required"