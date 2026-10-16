import {
  apiGet,
  apiPost,
  apiPut,
  apiDelete,
} from "./api";

export function getProducts() {
  return apiGet("/api/Products");
}

export function getProductById(id) {
  return apiGet(`/api/Products/${id}`);
}

export function createProduct(product) {
  return apiPost("/api/Products", product);
}

export function updateProduct(id, product) {
  return apiPut(`/api/Products/${id}`, product);
}

export function deleteProduct(id) {
  return apiDelete(`/api/Products/${id}`);
}