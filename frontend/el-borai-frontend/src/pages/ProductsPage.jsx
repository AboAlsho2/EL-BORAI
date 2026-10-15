
import { useEffect, useState } from "react";
import { Link } from "react-router";
import { useCart } from "../context/useCart";
import { getProducts } from "../services/productService";

export default function ProductsPage() {
  const { items, addItem } = useCart();

  const [products, setProducts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    let isActive = true;

    async function loadProducts() {
      try {
        const data = await getProducts();

        if (isActive) {
          setProducts(data);
        }
      } catch (err) {
        if (isActive) {
          setError(err.message || "Failed to load products.");
        }
      } finally {
        if (isActive) {
          setIsLoading(false);
        }
      }
    }

    loadProducts();

    return () => {
      isActive = false;
    };
  }, []);

  function handleAddToCart(product) {
    addItem({
      ...product,
      quantity: 1,
    });
  }

  if (isLoading) {
    return <main><p>Loading products...</p></main>;
  }

  if (error) {
    return (
      <main>
        <h1>Products</h1>
        <p role="alert">Error: {error}</p>
        <button onClick={() => window.location.reload()}>
          Try Again
        </button>
      </main>
    );
  }

  return (
    <main>
      <h1>Products</h1>

      {products.length === 0 ? (
        <p>No products found.</p>
      ) : (
        products.map((product) => (
          <article key={product.id}>
            <h2>
              <Link to={`/products/${product.id}`}>
                {product.name}
              </Link>
            </h2>
            <p>{product.description}</p>
            <p>Category: {product.categoryName}</p>
            <p>Price: {product.price} EGP</p>
            <p>Available stock: {product.stock}</p>

            <button
              onClick={() => handleAddToCart(product)}
              disabled={product.stock <= 0}
            >
              {product.stock <= 0 ? "Out of Stock" : "Add to Cart"}
            </button>
          </article>
        ))
      )}

      <p>Unique products in cart: {items.length}</p>
    </main>
  );
}
