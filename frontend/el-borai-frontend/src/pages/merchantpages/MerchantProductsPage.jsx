import { useEffect, useState } from "react";
import { Link } from "react-router";
import {
  getProducts,
  deleteProduct,
} from "../../services/productService";

export default function MerchantProductsPage() {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    let cancelled = false;

    async function loadProducts() {
      try {
        const data = await getProducts();

        if (!cancelled) {
          setProducts(data);
        }
      } catch (error) {
        if (!cancelled) {
          setError(error.message);
        }
      } finally {
        if (!cancelled) {
          setLoading(false);
        }
      }
    }

    loadProducts();

    return () => {
      cancelled = true;
    };
  }, []);

  async function handleDelete(id) {
    const confirmed = window.confirm(
      "Are you sure you want to delete this product?"
    );

    if (!confirmed) {
      return;
    }

    try {
      await deleteProduct(id);

      setProducts((currentProducts) =>
        currentProducts.filter((product) => product.id !== id)
      );
    } catch (error) {
      setError(error.message);
    }
  }

  if (loading) {
    return (
      <main>
        <h1>Merchant Products</h1>
        <p>Loading products...</p>
      </main>
    );
  }

  return (
    <main>
      <h1>Merchant Products</h1>

      <Link to="/merchant/products/new">
        Create Product
      </Link>

      {error && <p>{error}</p>}

      {products.length === 0 ? (
        <p>No products found.</p>
      ) : (
        <ul>
          {products.map((product) => (
            <li key={product.id}>
              <strong>{product.name}</strong>

              <span>
                {" - "}
                {product.price} EGP
              </span>

              <span>
                {" - "}
                Stock: {product.stock}
              </span>

              {" "}

              <Link to={`/merchant/products/${product.id}/edit`}>
                Edit
              </Link>

              {" "}

              <button onClick={() => handleDelete(product.id)}>
                Delete
              </button>
            </li>
          ))}
        </ul>
      )}
    </main>
  );
}