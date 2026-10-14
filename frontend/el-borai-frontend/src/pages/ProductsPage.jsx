
import { useCart } from "../context/useCart";

export default function ProductsPage() {
  const { items, addItem  } = useCart();

function addTestProduct() {
  const product = {
    id: 1,
    name: "Test Product",
    price: 100,
    quantity: 1,
  };

  addItem(product);
}
  return (
    <main>
      <h1>Products</h1>

      <button onClick={addTestProduct}>
        Add Test Product
      </button>

      <p>Unique products in cart: {items.length}</p>
    </main>
  );
}
