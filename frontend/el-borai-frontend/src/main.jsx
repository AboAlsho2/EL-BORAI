import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router";
import App from "./App.jsx";
import { CartProvider } from "./context/CartContext.jsx";
import AuthProvider from "./context/AuthProvider.jsx";
import keycloak from "./auth/keycloak";
import "./index.css";

const root = createRoot(document.getElementById("root"));

async function bootstrap() {
  try {
    await keycloak.init({
      onLoad: "check-sso",
      pkceMethod: "S256",
    });

    root.render(
      <StrictMode>
        <BrowserRouter>
          <AuthProvider>
            <CartProvider>
              <App />
            </CartProvider>
          </AuthProvider>
        </BrowserRouter>
      </StrictMode>
    );
  } catch (error) {
    console.error("Keycloak initialization failed:", error);

    root.render(
      <main>
        <h1>Authentication initialization failed</h1>
        <p>
          Unable to initialize Keycloak. Check that the Keycloak
          server is running and the client configuration is correct.
        </p>
      </main>
    );
  }
}

bootstrap();