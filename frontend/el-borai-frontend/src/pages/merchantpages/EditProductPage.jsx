
import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router";
import {
  getProductById,
  updateProduct,
} from "../../services/productService";
import ProductForm from "../../components/ProductForm";

export default function EditProductPage() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [initialValues, setInitialValues] = useState(null);
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    let cancelled = false;

    async function loadProduct() {
      try {
        const product = await getProductById(id);

        if (cancelled) return;

        setInitialValues({
          name: product.name ?? "",
          description: product.description ?? "",
          price: String(product.price ?? ""),
          stock: String(product.stock ?? ""),
          categoryId: String(product.categoryId ?? ""),
        });
      } catch (error) {
        if (!cancelled) {
          setError(error.message);
        }
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    }

    loadProduct();

    return () => {
      cancelled = true;
    };
  }, [id]);

  async function handleUpdate(product) {
    setError("");
    setIsSubmitting(true);

    try {
      await updateProduct(id, product);
      navigate("/merchant/products");
    } catch (error) {
      setError(error.message);
    } finally {
      setIsSubmitting(false);
    }
  }

  if (isLoading) {
    return (
      <main>
        <h1>Edit Product</h1>
        <p>Loading product...</p>
      </main>
    );
  }

  if (!initialValues) {
    return (
      <main>
        <h1>Edit Product</h1>
        <p role="alert">{error || "Product could not be loaded."}</p>
        <Link to="/merchant/products">Back to products</Link>
      </main>
    );
  }

  return (
    <main>
      <h1>Edit Product</h1>

      <ProductForm
        initialValues={initialValues}
        onSubmit={handleUpdate}
        submitLabel="Update Product"
        isSubmitting={isSubmitting}
        error={error}
      />

      <p>
        <Link to="/merchant/products">Cancel</Link>
      </p>
    </main>
  );
}
