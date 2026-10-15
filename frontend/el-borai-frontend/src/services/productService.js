
import { apiGet } from "./api";

export function getProducts() {
  return apiGet("/api/Products");
}
export function getProductById(id) {
  return apiGet(`/api/Products/${id}`);
}