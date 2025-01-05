Feature: API Operations for Products

  Scenario: Retrieving all products from the API
    Given there are products with IDs 1, 2, 3 in the database
    When a GET request is made to the endpoint `api/products`
    Then the response should contain all the products
    And the products should have correct data like name, price, and quantity

  Scenario: Adding a product via the API
    Given the product "Monitor" does not exist in the database
    When a POST request is made to the endpoint `api/products` with data "Name": "Monitor", "Price": 500
    Then the response should have a status code 201 Created
    And the product "Monitor" should be added to the database

  Scenario: Editing a product via the API
    Given the product with ID 2 exists in the database with the name "Laptop"
    When a PUT request is made to the endpoint `api/products/2` with new data ("Name": "Laptop Pro", "Price": 1200)
    Then the response to the PUT request should have a status code 204 No Content
    And the product "Laptop" should be updated to "Laptop Pro" with a new price of 1200

  Scenario: Deleting a product via the API
    Given the product with ID 3 exists in the database
    When a DELETE request is made to the endpoint `api/products/3`
    Then the response to the DELETE request should have a status code 204 No Content
    And the product with ID 3 should be deleted from the database

   Scenario: Handling errors when deleting a non-existing product
    Given the product with ID 10 does not exist in the database
    When a DELETE request is made to the endpoint `api/products/10`
    Then the response should have a status code 404 Not Found
    And the product with ID 10 should not be deleted

Scenario: Trying to edit a non-existing product
  Given the product with ID 100 does not exist in the database
  When a PUT request is made to the endpoint `api/products/100` with new data ("Name": "Non-existent", "Price": 1500)
  Then the response should have a status code 404 Not Found
  And the product should not be modified

Scenario: Trying to retrieve a non-existing product
  Given the product with ID 100 does not exist in the database
  When a GET request is made to the endpoint `api/products/100`
  Then the response should have a status code 404 Not Found
  And the response body should contain an error message

Scenario: Retrieving products when the database is empty
  Given the database is empty
  When a GET request is made to the endpoint `api/products`
  Then the response should contain an empty list

Scenario: Trying to delete a product that was already deleted
  Given the product with ID 3 exists in the database
  When a DELETE request is made to the endpoint `api/products/3`
  Then the response should have a status code 204 No Content
  And the product with ID 3 should be deleted from the database
  When a DELETE request is made to the endpoint `api/products/3` again
  Then the response should have a status code 404 Not Found

Scenario: Trying to update a product with invalid data
  Given the product with ID 1 exists in the database with the name "Monitor"
  When a PUT request is made to the endpoint `api/products/1` with invalid data (Name: "Monitor Pro", Price: "Invalid Price", Quantity: 5)
  Then the response should have a status code 400 Bad Request
  And the product should not be updated