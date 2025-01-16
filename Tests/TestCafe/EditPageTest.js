import { Selector } from 'testcafe';

fixture('Edit Product Page')
    .page('https://localhost:7086/edit-product/46');

test('Edit and Save Product', async t => {
    const productName = Selector('#productName');
    const productPrice = Selector('#productPrice');
    const productCategory = Selector('#productCategory');
    const stockQuantity = Selector('#stockQuantity');
    const saveButton = Selector('button').withText('Save Changes');
    const successMessage = Selector('.alert-success');

    // Wypełnij pola formularza
    await t
        .typeText(productName, 'Updated Product')
        .typeText(productPrice, '20')
        .typeText(productCategory, 'Updated Category')
        .typeText(stockQuantity, '10')

    // Kliknij przycisk zapisu
    await t
        .click(saveButton)

    // Sprawdź, czy pojawił się komunikat sukcesu
    await t
        .expect(successMessage.exists).ok('Success message should appear after saving');
});
