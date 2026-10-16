
import { useCart } from "../../context/useCart";

export default function CartPage() {
  const {
    items,
    removeItem,
    updateQuantity,
    clearCart,
    isLoading,
    error,
  } = useCart();

  const totalQuantity = items.reduce(
    (total, item) => total + item.quantity,
    0
  );

  const totalPrice = items.reduce(
    (total, item) => total + item.price * item.quantity,
    0
  );

  const formatPrice = (price) =>
    new Intl.NumberFormat("en-EG", {
      style: "currency",
      currency: "EGP",
    }).format(price);

  return (
    <main>
      <h1>Shopping Cart</h1>

      {error && <p role="alert">{error}</p>}

      {isLoading ? (
        <p>Loading cart...</p>
      ) : items.length === 0 ? (
        <p>Your cart is empty.</p>
      ) : (
        <>
          {items.map((item) => (
            <div key={item.id}>
              <h2>{item.name}</h2>

              <p>Price: {formatPrice(item.price)}</p>
              <p>Quantity: {item.quantity}</p>
              <p>
                Subtotal: {formatPrice(item.price * item.quantity)}
              </p>

              <button
                type="button"
                disabled={isLoading}
                onClick={() =>
                  updateQuantity(item.id, item.quantity - 1)
                }
              >
                -
              </button>

              <button
                type="button"
                disabled={isLoading}
                onClick={() =>
                  updateQuantity(item.id, item.quantity + 1)
                }
              >
                +
              </button>

              <button
                type="button"
                disabled={isLoading}
                onClick={() => removeItem(item.id)}
              >
                Remove
              </button>
            </div>
          ))}

          <hr />

          <p>Total quantity: {totalQuantity}</p>
          <p>Total price: {formatPrice(totalPrice)}</p>

          <button
            type="button"
            disabled={isLoading}
            onClick={clearCart}
          >
            Clear Cart
          </button>
        </>
      )}
    </main>
  );
}
