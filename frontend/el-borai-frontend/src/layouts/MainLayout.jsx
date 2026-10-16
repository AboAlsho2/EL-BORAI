import { Outlet } from "react-router";
import Navbar from "../components/Navbar";
import useAuth from "../context/useAuth";

export default function MainLayout() {
  const { isAuthenticated, user, login, logout } = useAuth();

  return (
    <>
      <Navbar />

      <section
        style={{
          padding: "12px 16px",
          background: "#f3f4f6",
          display: "flex",
          alignItems: "center",
          gap: "12px",
          flexWrap: "wrap",
        }}
      >
        <div>
          <strong>Auth Status:</strong>{" "}
          {isAuthenticated ? "Authenticated" : "Not authenticated"}

          {user && (
            <span>
              {" | "}
              User: {user.preferred_username ?? user.name ?? user.sub}
            </span>
          )}
        </div>

        {isAuthenticated ? (
          <button onClick={logout}>Logout</button>
        ) : (
          <button onClick={login}>Login</button>
        )}
      </section>

      <main>
        <Outlet />
      </main>
    </>
  );
}