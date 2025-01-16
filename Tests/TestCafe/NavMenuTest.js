import { Selector } from 'testcafe';

fixture`Navigation Menu Tests`
    .page`https://localhost:7086`;

test('Navigate to Home', async t => {
    const homeLink = Selector('.nav-link').withText('Home');

    await t.click(homeLink);

    // Sprawdź adres URL
    await t.expect(t.eval(() => window.location.pathname)).eql('/');
});

test('Navigate to Add Product', async t => {
    const addProductLink = Selector('.nav-link').withText('Add Product');

    await t.click(addProductLink);

    // Sprawdź adres URL
    await t.expect(t.eval(() => window.location.pathname)).eql('/add-product');
});

test('Navigate to Products', async t => {
    const productsLink = Selector('.nav-link').withText('Products');

    await t.click(productsLink);

    // Sprawdź adres URL
    await t.expect(t.eval(() => window.location.pathname)).eql('/products');
});
