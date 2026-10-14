
import { useCart } from "../context/useCart";

export default function CartPage() {
  const {
    items,
    removeItem,
    updateQuantity,
    clearCart,
  } = useCart();

  const totalQuantity = items.reduce(
    (total, item) => total + item.quantity,
    0
  );

  const totalPrice = items.reduce(
    (total, item) => total + item.price * item.quantity,
    0
  );

  return (
    <main>
      <h1>Shopping Cart</h1>

      {items.length === 0 ? (
        <p>Your cart is empty.</p>
      ) : (
        <>
      {items.map((item) => (
        <div key={item.id}>
          <h2>{item.name}</h2>

          <p>Price: {item.price}</p>

          <p>Quantity: {item.quantity}</p>

          <button
            onClick={() =>
              updateQuantity(item.id, item.quantity - 1)
            }
          >
            -
          </button>

          <button
            onClick={() =>
              updateQuantity(item.id, item.quantity + 1)
            }
          >
            +
          </button>

          <button
            onClick={() => removeItem(item.id)}
          >
            Remove
          </button>
        </div>
      ))}

          <p>Total quantity: {totalQuantity}</p>
          <p>Total price: {totalPrice}</p>
          <button onClick={clearCart}>
            Clear Cart
          </button>
        </>
      )}
    </main>
  );
}
