import { Selector } from 'testcafe';

fixture`Additional Product Page Tests`
    .page`https://localhost:7086/add-product`;

// Test: Should show success message after registration
test('Should show success message after registration', async t => {
    await t
        .typeText('#productName', 'Registered Product')
        .typeText('#productDate', '2025-01-15')
        .typeText('#productPrice', '20')
        .typeText('#productCategory', 'Category B')
        .typeText('#stockQuantity', '50')
        .click('button.btn.btn-primary');

    const successMessage = Selector('div.alert.alert-success');
    await t.expect(successMessage.innerText).contains('successfully', 'No success message displayed.');
});



