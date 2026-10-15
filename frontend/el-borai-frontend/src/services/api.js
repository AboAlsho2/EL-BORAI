
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function apiGet(endpoint) {
  if (!API_BASE_URL) {
    throw new Error(
      "VITE_API_BASE_URL is missing. Check your .env file."
    );
  }

  const baseUrl = API_BASE_URL.replace(/\/+$/, "");
  const response = await fetch(`${baseUrl}${endpoint}`);

  if (!response.ok) {
    throw new Error(
      `API request failed: ${response.status} ${response.statusText}`
    );
  }

  return response.json();
}
