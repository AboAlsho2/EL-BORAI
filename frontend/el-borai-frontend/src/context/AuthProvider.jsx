import { useEffect, useState } from "react";
import AuthContext from "./AuthContext";
import keycloak from "../auth/keycloak";

export default function AuthProvider({ children }) {
  const [isAuthenticated, setIsAuthenticated] = useState(
    Boolean(keycloak.authenticated)
  );

  const [user, setUser] = useState(
    keycloak.tokenParsed ?? null
  );

  useEffect(() => {
    function syncAuthState() {
      setIsAuthenticated(Boolean(keycloak.authenticated));
      setUser(keycloak.authenticated ? keycloak.tokenParsed ?? null : null);
    }

    keycloak.onAuthSuccess = syncAuthState;
    keycloak.onAuthLogout = syncAuthState;
    keycloak.onAuthRefreshSuccess = syncAuthState;
    keycloak.onAuthRefreshError = syncAuthState;

    syncAuthState();

    return () => {
      keycloak.onAuthSuccess = undefined;
      keycloak.onAuthLogout = undefined;
      keycloak.onAuthRefreshSuccess = undefined;
      keycloak.onAuthRefreshError = undefined;
    };
  }, []);

  async function login() {
    await keycloak.login();
  }

  async function logout() {
    await keycloak.logout({
      redirectUri: window.location.origin,
    });
  }

  const value = {
    isAuthenticated,
    user,
    login,
    logout,
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}