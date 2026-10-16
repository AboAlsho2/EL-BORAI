
import { useState } from "react";
import { Link, useNavigate } from "react-router";
import { createProduct } from "../../services/productService";
import ProductForm from "../../components/ProductForm";

const initialForm = {
  name: "",
  description: "",
  price: "",
  stock: "",
  categoryId: "",
};

export default function CreateProductPage() {
  const [error, setError] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const navigate = useNavigate();

  async function handleCreate(product) {
    setError("");
    setIsSubmitting(true);

    try {
      await createProduct(product);
      navigate("/merchant/products");
    } catch (error) {
      setError(error.message);
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <main>
      <h1>Create Product</h1>

      <ProductForm
        initialValues={initialForm}
        onSubmit={handleCreate}
        submitLabel="Create Product"
        isSubmitting={isSubmitting}
        error={error}
      />

      <p>
        <Link to="/merchant/products">Cancel</Link>
      </p>
    </main>
  );
}
