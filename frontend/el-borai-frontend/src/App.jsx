
import { Route, Routes } from "react-router";

import MainLayout from "./layouts/MainLayout";
import HomePage from "./pages/HomePage";
import ProductsPage from "./pages/ProductsPage";
import ProductDetailsPage from "./pages/ProductDetailsPage";
import CartPage from "./pages/CartPage";
import ComingSoonPage from "./pages/ComingSoonPage";
import NotFoundPage from "./pages/NotFoundPage";

export default function App() {
  return (
    <Routes>
      <Route element={<MainLayout />}>
        <Route path="/" element={<HomePage />} />

        <Route path="/products" element={<ProductsPage />} />
        <Route
          path="/products/:id"
          element={<ProductDetailsPage />}
        />

        <Route path="/cart" element={<CartPage />} />

        <Route
          path="/checkout"
          element={<ComingSoonPage title="Checkout" />}
        />

        <Route
          path="/orders"
          element={<ComingSoonPage title="My Orders" />}
        />

        <Route
          path="/orders/:id"
          element={<ComingSoonPage title="Order Details" />}
        />

        <Route
          path="/merchant/products"
          element={<ComingSoonPage title="Merchant Products" />}
        />

        <Route
          path="/merchant/products/new"
          element={<ComingSoonPage title="Create Product" />}
        />

        <Route
          path="/merchant/products/:id/edit"
          element={<ComingSoonPage title="Edit Product" />}
        />

        <Route path="*" element={<NotFoundPage />} />
      </Route>
    </Routes>
  );
}
