Feature: Edit Product

  Scenario: Editing an existing product
    Given the user is on the "Edit Product" page
    When the user changes the product name to "Smartphone Pro" and the price to 1200
    And clicks the "Save" button
    Then the product "Smartphone Pro" with price 1200 should be saved
    And a success message should appear

  Scenario: Editing a non-existing product
    Given the user wants to edit a product with ID 99 that does not exist in the database
    When the user submits the edit form
    Then the user should see the error message "Product does not exist"
    And the product should not be updated

   Scenario: Editing a product with missing name
    Given the user is on the "Edit Product" page
    When the user submits the form with missing "Name" field
    Then the product should not be updated
    And the user should see the message "Name is required"

  Scenario: Editing a product with negative price
    Given the user is on the "Edit Product" page
    When the user submits the form with a negative "Price" (-100)
    Then the product should not be updated
    And the user should see the message "Price must be greater than 0"

  Scenario: Editing a product with invalid quantity (negative)
    Given the user is on the "Edit Product" page
    When the user submits the form with a negative "Quantity" (-10)
    Then the product should not be updated
    And the user should see the message "Quantity cannot be negative"

  Scenario: Editing a product with an empty price
    Given the user is on the "Edit Product" page
    When the user submits the form with an empty "Price" field
    Then the product should not be updated
    And the user should see the message "Price is required"