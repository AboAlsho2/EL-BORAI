import { useState } from "react";
import { CartContext } from "./cart-context";

export function CartProvider({ children }) {
  const [items, setItems] = useState([]);

  function addItem(product) {
    setItems((currentItems) => {
      const existingItem = currentItems.find(
        (item) => item.id === product.id
      );

      if (existingItem) {
        return currentItems.map((item) =>
          item.id === product.id
            ? { ...item, quantity: item.quantity + 1 }
            : item
        );
      }

      return [...currentItems, product];
    });
  }

  function removeItem(productId) {
    setItems((currentItems) =>
      currentItems.filter((item) => item.id !== productId)
    );
  }

  function updateQuantity(productId, quantity) {
      if (quantity <= 0) {
        removeItem(productId);
        return;
      }
    
    setItems((currentItems) =>
      currentItems.map((item) =>
        item.id === productId
          ? { ...item, quantity }
          : item
      )
    );
  }

  function clearCart() {
    setItems([]);
  }

  return (
    <CartContext.Provider
      value={{
        items,
        addItem,
        removeItem,
        updateQuantity,
        clearCart,
      }}
    >
      {children}
    </CartContext.Provider>
  );
}