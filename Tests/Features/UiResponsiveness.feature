Feature: UI Responsiveness

  Scenario: Checking the responsiveness of the product form
    Given the user is on the "Add Product" page
    When the user resizes the window to a smaller size (e.g., 800x600)
    Then the form should adjust to the new screen size
    And all form fields should remain visible and interactive

    When the user resizes the window to a larger size (e.g., 1200x800)
    Then the form should adjust to the new screen size
    And all form fields should remain visible and interactive
