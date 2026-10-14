
import { Route, Routes } from 'react-router'
import MainLayout from './layouts/MainLayout'
import PlaceholderPage from './pages/PlaceholderPage'
export default function App() {
  return (
    <Routes>
      <Route element={<MainLayout />}>
        <Route
          path="/"
          element={
            <PlaceholderPage
              title="EL-BORAI"
              description="Welcome to EL-BORAI."
            />
          }
        />

        <Route
          path="/products"
          element={
            <PlaceholderPage
              title="Products"
              description="Browse available products."
            />
          }
        />

        <Route
          path="/products/:id"
          element={
            <PlaceholderPage
              title="Product Details"
              description="View product information."
            />
          }
        />

        <Route
          path="/cart"
          element={<PlaceholderPage title="Cart" />}
        />

        <Route
          path="/checkout"
          element={<PlaceholderPage title="Checkout" />}
        />

        <Route
          path="/orders"
          element={<PlaceholderPage title="My Orders" />}
        />

        <Route
          path="/orders/:id"
          element={<PlaceholderPage title="Order Details" />}
        />

        <Route
          path="/merchant/products"
          element={<PlaceholderPage title="Merchant Products" />}
        />

        <Route
          path="/merchant/products/new"
          element={<PlaceholderPage title="Create Product" />}
        />

        <Route
          path="/merchant/products/:id/edit"
          element={<PlaceholderPage title="Edit Product" />}
        />

        <Route
          path="*"
          element={
            <PlaceholderPage
              title="404 - Not Found"
              description="The requested page does not exist."
            />
          }
        />
      </Route>
    </Routes>
  )
}
