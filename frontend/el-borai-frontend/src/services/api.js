import keycloak from "../auth/keycloak";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

async function request(endpoint, options = {}) {
  if (!API_BASE_URL) {
    throw new Error(
      "VITE_API_BASE_URL is missing. Check your .env file."
    );
  }

  const baseUrl = API_BASE_URL.replace(/\/+$/, "");
  const headers = {
    ...options.headers,
  };

  if (keycloak.authenticated) {
    try {
      await keycloak.updateToken(30);
    } catch {
      throw new Error(
        "Your session could not be refreshed. Please log in again."
      );
    }

    if (!keycloak.token) {
      throw new Error(
        "Access token is unavailable. Please log in again."
      );
    }

    headers.Authorization = `Bearer ${keycloak.token}`;
  }

  let response;

  try {
    response = await fetch(`${baseUrl}${endpoint}`, {
      ...options,
      headers,
    });
  } catch {
    throw new Error(
      "Unable to connect to the server. Check your connection and API URL."
    );
  }

  if (!response.ok) {
    const message = `API request failed: ${response.status} ${response.statusText}`;
    throw new Error(message);
  }

  if (response.status === 204) {
    return null;
  }

  const contentType = response.headers.get("content-type");

  if (contentType?.includes("application/json")) {
    return response.json();
  }

  return null;
}

export function apiGet(endpoint) {
  return request(endpoint);
}

export function apiPost(endpoint, body) {
  return request(endpoint, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });
}

export function apiPut(endpoint, body) {
  return request(endpoint, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });
}

export function apiDelete(endpoint) {
  return request(endpoint, {
    method: "DELETE",
  });
}