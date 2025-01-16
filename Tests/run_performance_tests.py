import time
from statistics import mean
from typing import Callable

# Simulate critical application methods for benchmarking
def get_products():
    time.sleep(0.05)  # Simulate delay for getting products
    return "List of products"

def get_product_by_id(product_id):
    time.sleep(0.03)  # Simulate delay for getting a single product
    return f"Product {product_id}"

def add_product(product):
    time.sleep(0.07)  # Simulate delay for adding a product
    return True

def edit_product(product_id, product):
    time.sleep(0.06)  # Simulate delay for editing a product
    return True

def delete_product(product_id):
    time.sleep(0.04)  # Simulate delay for deleting a product
    return True

def validate_product(product):
    time.sleep(0.02)  # Simulate delay for validation
    return True

def setup_database():
    time.sleep(0.1)  # Simulate database setup
    return True

def configure_schema():
    time.sleep(0.08)  # Simulate schema configuration
    return True

def selenium_setup():
    time.sleep(0.05)  # Simulate Selenium test setup
    return True

def selenium_teardown():
    time.sleep(0.03)  # Simulate Selenium test teardown
    return True

def selenium_add_product():
    time.sleep(0.07)  # Simulate adding a product via Selenium
    return True

def selenium_display_products():
    time.sleep(0.06)  # Simulate displaying products via Selenium
    return True

def selenium_delete_product():
    time.sleep(0.04)  # Simulate deleting a product via Selenium
    return True

def selenium_navigate_to_edit():
    time.sleep(0.05)  # Simulate navigating to edit page
    return True

def performance_test_case():
    """
    Simulate processing data exceeding documented ranges to test overload handling.
    """
    large_data = "a" * 10**7  # 10 MB of string data
    try:
        processed_data = large_data.upper()
        assert processed_data is not None, "Data processing returned None."
        print("Overload test passed.")
    except Exception as e:
        print(f"Overload test failed: {e}")

def micro_benchmark():
    """
    Measure execution times of key methods and calculate average time.
    """
    methods_to_test = [
        lambda: get_products(),
        lambda: get_product_by_id(1),
        lambda: add_product({"name": "Test Product"}),
        lambda: edit_product(1, {"name": "Updated Product"}),
        lambda: delete_product(1),
        lambda: validate_product({"name": "Valid Product"}),
        lambda: setup_database(),
        lambda: configure_schema(),
        lambda: selenium_setup(),
        lambda: selenium_teardown(),
        lambda: selenium_add_product(),
        lambda: selenium_display_products(),
        lambda: selenium_delete_product(),
        lambda: selenium_navigate_to_edit(),
        lambda: get_products(),
        lambda: get_product_by_id(2),
        lambda: add_product({"name": "Another Product"}),
        lambda: edit_product(2, {"name": "Another Updated Product"}),
        lambda: delete_product(2),
        lambda: validate_product({"name": "Another Valid Product"})
    ]

    results = {}
    for idx, method in enumerate(methods_to_test):
        times = []
        for _ in range(20):
            start_time = time.time()
            method()
            times.append(time.time() - start_time)
        results[f"Method {idx + 1}"] = mean(times)

    print("Microbenchmark Results:")
    for method, avg_time in results.items():
        print(f"{method}: {avg_time:.6f} seconds")

if __name__ == "__main__":
    print("Running performance tests...")

    # Test overload handling
    performance_test_case()

    # Run microbenchmark tests
    micro_benchmark()