
import {
  apiGet,
  apiPost,
  apiPut,
  apiDelete,
} from "./api";

// GET /api/Cart
export async function getCart() {
  try {
    return await apiGet("/api/Cart");
  } catch (error) {
    // The backend returns 404 when the user has no cart yet.
    if (error.message.includes("404")) {
      return null;
    }

    throw error;
  }
}

// POST /api/Cart/items
export function addCartItem(productId, quantity = 1) {
  return apiPost("/api/Cart/items", {
    productId,
    quantity,
  });
}

// PUT /api/Cart/items/{productId}
export function updateCartItem(productId, quantity) {
  return apiPut(`/api/Cart/items/${productId}`, {
    quantity,
  });
}

// DELETE /api/Cart/items/{productId}
export function removeCartItem(productId) {
  return apiDelete(`/api/Cart/items/${productId}`);
}

// DELETE /api/Cart
export function clearServerCart() {
  return apiDelete("/api/Cart");
}
