
import { Route, Routes } from "react-router";

import MainLayout from "./layouts/MainLayout";
import HomePage from "./pages/HomePage";
import ProductsPage from "./pages/productpages/ProductsPage";
import ProductDetailsPage from "./pages/productpages/ProductDetailsPage";
import CartPage from "./pages/CartPage";
import ComingSoonPage from "./pages/ComingSoonPage";
import NotFoundPage from "./pages/NotFoundPage";
import MerchantProductsPage from "./pages/merchantpages/MerchantProductsPage";
import CreateProductPage from "./pages/merchantpages/CreateProductPage";
import EditProductPage from "./pages/merchantpages/EditProductPage";
import ProtectedRoute from "./components/ProtectedRoute";

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
          element={
            <ProtectedRoute requiredRole="CUSTOMER">
              <ComingSoonPage title="Checkout" />
            </ProtectedRoute>
          }
        />

        <Route
          path="/orders"
          element={
            <ProtectedRoute requiredRole="CUSTOMER">
              <ComingSoonPage title="My Orders" />
            </ProtectedRoute>
          }
        />

        <Route
          path="/orders/:id"
          element={
            <ProtectedRoute requiredRole="CUSTOMER">
              <ComingSoonPage title="Order Details" />
            </ProtectedRoute>
          }
        />

        <Route
          path="/merchant/products"
          element={
            <ProtectedRoute requiredRole="MERCHANT">
              <MerchantProductsPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/merchant/products/new"
          element={
            <ProtectedRoute requiredRole="MERCHANT">
              <CreateProductPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/merchant/products/:id/edit"
          element={
            <ProtectedRoute requiredRole="MERCHANT">
              <EditProductPage />
            </ProtectedRoute>
          }
        />

        <Route path="*" element={<NotFoundPage />} />
      </Route>
    </Routes>
  );
}
