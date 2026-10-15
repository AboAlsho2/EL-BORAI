
import { useEffect, useState } from "react";
import { Link, useParams } from "react-router";
import { useCart } from "../context/useCart";
import { getProductById } from "../services/productService";

export default function ProductDetailsPage() {
  const { id } = useParams();
  const { addItem } = useCart();

  const [product, setProduct] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    let isActive = true;

    async function loadProduct() {
      setIsLoading(true);
      setError("");
      setProduct(null);

      try {
        const data = await getProductById(id);

        if (isActive) {
          setProduct(data);
        }
      } catch (err) {
        if (isActive) {
          setError(err.message || "Failed to load product.");
        }
      } finally {
        if (isActive) {
          setIsLoading(false);
        }
      }
    }

    loadProduct();

    return () => {
      isActive = false;
    };
  }, [id]);

  if (isLoading) {
    return <main><p>Loading product...</p></main>;
  }

  if (error) {
    return (
      <main>
        <h1>Product Details</h1>
        <p role="alert">Could not load product: {error}</p>
        <Link to="/products">Back to Products</Link>
      </main>
    );
  }

  if (!product) {
    return (
      <main>
        <h1>Product Not Found</h1>
        <Link to="/products">Back to Products</Link>
      </main>
    );
  }

  function handleAddToCart() {
    addItem({
      ...product,
      quantity: 1,
    });
  }

  return (
    <main>
      <Link to="/products">← Back to Products</Link>

      <article>
        <h1>{product.name}</h1>
        <p>{product.description}</p>
        <p>Category: {product.categoryName}</p>
        <p>Price: {product.price} EGP</p>
        <p>Available stock: {product.stock}</p>

        <button
          onClick={handleAddToCart}
          disabled={product.stock <= 0}
        >
          {product.stock <= 0 ? "Out of Stock" : "Add to Cart"}
        </button>
      </article>
    </main>
  );
}
