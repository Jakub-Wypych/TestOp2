Feature: Edit Product

  Scenario: Successfully editing an existing product
    Given the product "Smartphone" with ID 1 exists in the database
    When the user updates the product with name "Smartphone Pro" and price 1200
    Then the service should successfully save the product "Smartphone Pro" with price 1200

  Scenario: Attempting to edit a non-existing product
    Given the product with ID -1 does not exist
    When the user attempts to edit the product with ID -1
    Then the service should return the message "Product does not exist"
    And the product should not be updated

  Scenario: Attempting to edit a product with missing name
    Given the product "Smartphone" with ID 1 exists in the database
    When the user submits the product update with missing Name field
    Then the service should return the message "Name is required"
    And the product should not be updated

  Scenario: Attempting to edit a product with a negative price
    Given the product "Smartphone" with ID 1 exists in the database
    When the user submits the product update with negative Price (-100)
    Then the service should return the message "Price must be greater than 0"
    And the product should not be updated

  Scenario: Attempting to edit a product with negative quantity
    Given the product "Smartphone" with ID 1 exists in the database
    When the user submits the product update with negative Quantity (-10)
    Then the service should return the message "Quantity cannot be negative"
    And the product should not be updated