
import useAuth from "../context/useAuth";

export default function ProtectedRoute({ children, requiredRole }) {
  const { isAuthenticated, user, login } = useAuth();

  if (!isAuthenticated) {
    return (
      <main>
        <h1>Login Required</h1>
        <p>You must log in to access this page.</p>
        <button type="button" onClick={login}>
          Login with Keycloak
        </button>
      </main>
    );
  }

  const realmRoles = user?.realm_access?.roles ?? [];
  const clientRoles =
    user?.resource_access?.["elborai-frontend"]?.roles ?? [];

  const hasRequiredRole =
    !requiredRole ||
    realmRoles.includes(requiredRole) ||
    clientRoles.includes(requiredRole);

  if (!hasRequiredRole) {
    return (
      <main>
        <h1>Access Denied</h1>
        <p>You do not have permission to access this page.</p>
        <button
          type="button"
          onClick={() => window.location.assign("/")}
        >
          Go Home
        </button>
      </main>
    );
  }

  return children;
}
