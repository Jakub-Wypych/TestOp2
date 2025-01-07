Feature: Delete Product

  Scenario: Successfully deleting an existing product
    Given the product "Smartphone" exists in the list of products
    When the user clicks the Delete button next to the product "Smartphone"
    Then the service should successfully remove the product "Smartphone" from the list
    And a success message should appear

  Scenario: Successfully deleting multiple products individually
    Given the products "Smartphone", "Tablet", and "Laptop" exist in the list of products
    When the user clicks the Delete button next to the product "Smartphone"
    Then the service should successfully remove the product "Smartphone" from the list
    And a success message should appear

    When the user clicks the Delete button next to the product "Tablet"
    Then the service should successfully remove the product "Tablet" from the list
    And a success message should appear

    When the user clicks the Delete button next to the product "Laptop"
    Then the service should successfully remove the product "Laptop" from the list
    And a success message should appear

  Scenario: Attempting to delete a non-existing product
    Given the product with ID -1 does not exist
    When the user attempts to delete the product with ID -1
    Then service should return the message "Product does not exist"
    And no product should be removed from the list
