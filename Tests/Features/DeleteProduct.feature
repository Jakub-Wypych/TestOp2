Feature: Delete Product

  Scenario: Deleting an existing product
    Given the user sees a list of products, including the product "Smartphone"
    When the user clicks the "Delete" button next to "Smartphone"
    Then the product "Smartphone" should be removed from the list of products
    And a success message should appear

  Scenario: Deleting multiple products individually
    Given the user sees a list of products, including the products "Smartphone", "Tablet", and "Laptop"
    When the user clicks the "Delete" button next to "Smartphone"
    Then the product "Smartphone" should be removed from the list of products
    And a success message should appear

    When the user clicks the "Delete" button next to "Tablet"
    Then the product "Tablet" should be removed from the list of products
    And a success message should appear

    When the user clicks the "Delete" button next to "Laptop"
    Then the product "Laptop" should be removed from the list of products
    And a success message should appear