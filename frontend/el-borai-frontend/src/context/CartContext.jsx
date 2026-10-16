
import { useCallback, useEffect,  useRef, useState } from "react";
import { CartContext } from "./cart-context";
import  useAuth  from "./useAuth";
import {
  getCart,
  addCartItem,
  updateCartItem,
  removeCartItem,
  clearServerCart,
} from "../services/cartService";

export function CartProvider({ children }) {
  const { isAuthenticated } = useAuth();

  const [items, setItems] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");


const requestId = useRef(0);

const loadCart = useCallback(async () => {
  if (!isAuthenticated) {
    setItems([]);
    setError("");
    setIsLoading(false);
    return;
  }

  const currentRequestId = ++requestId.current;

  setIsLoading(true);
  setError("");

  try {
    const cart = await getCart();

    if (currentRequestId !== requestId.current) return;

    setItems(
      (cart?.items ?? []).map((item) => ({
        id: item.productId,
        name: item.productName,
        price: item.unitPrice,
        quantity: item.quantity,
      }))
    );
  } catch (err) {
    if (currentRequestId !== requestId.current) return;
    setError(err.message || "Failed to load cart.");
  } finally {
    if (currentRequestId === requestId.current) {
      setIsLoading(false);
    }
  }
}, [isAuthenticated]);

useEffect(() => {
  if (!isAuthenticated) {
    return;
  }

  let cancelled = false;

  async function fetchCart() {
    setIsLoading(true);
    setError("");

    try {
      const cart = await getCart();

      if (cancelled) return;

      setItems(
        (cart?.items ?? []).map((item) => ({
          id: item.productId,
          name: item.productName,
          price: item.unitPrice,
          quantity: item.quantity,
        }))
      );
    } catch (err) {
      if (!cancelled) {
        setError(err.message || "Failed to load cart.");
      }
    } finally {
      if (!cancelled) {
        setIsLoading(false);
      }
    }
  }

  fetchCart();

  return () => {
    cancelled = true;
  };
}, [isAuthenticated]);


  const addItem = useCallback(
    async (product) => {
      if (!isAuthenticated) {
        setError("Please log in to add products to your cart.");
        return;
      }

      setError("");

      try {
        await addCartItem(product.id, product.quantity ?? 1);
        await loadCart();
      } catch (err) {
        setError(err.message || "Failed to add product to cart.");
        throw err;
      }
    },
    [isAuthenticated, loadCart]
  );

  const removeItem = useCallback(
    async (productId) => {
      setError("");

      try {
        await removeCartItem(productId);
        await loadCart();
      } catch (err) {
        setError(err.message || "Failed to remove product.");
      }
    },
    [loadCart]
  );

  const updateQuantity = useCallback(
    async (productId, quantity) => {
      if (quantity <= 0) {
        await removeItem(productId);
        return;
      }

      setError("");

      try {
        await updateCartItem(productId, quantity);
        await loadCart();
      } catch (err) {
        setError(err.message || "Failed to update quantity.");
      }
    },
    [loadCart, removeItem]
  );

  const clearCart = useCallback(async () => {
    setError("");

    try {
      await clearServerCart();
      setItems([]);
    } catch (err) {
      setError(err.message || "Failed to clear cart.");
    }
  }, []);

  return (
    <CartContext.Provider
      value={{
        items,
        addItem,
        removeItem,
        updateQuantity,
        clearCart,
        isLoading,
        error,
        refreshCart: loadCart,
      }}
    >
      {children}
    </CartContext.Provider>
  );
}
