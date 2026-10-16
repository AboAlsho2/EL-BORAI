
import { NavLink } from "react-router";
import useAuth from "../context/useAuth";

const links = [
  { to: "/", label: "Home", end: true },
  { to: "/products", label: "Products" },
  { to: "/cart", label: "Cart" },
];

export default function Navbar() {
  const { isAuthenticated, user, login, logout } = useAuth();

  const realmRoles = user?.realm_access?.roles ?? [];
  const clientRoles =
    user?.resource_access?.["elborai-frontend"]?.roles ?? [];

  const isMerchant =
    realmRoles.includes("MERCHANT") ||
    clientRoles.includes("MERCHANT");

  const isCustomer =
    realmRoles.includes("CUSTOMER") ||
    clientRoles.includes("CUSTOMER");

  return (
    <nav>
      <NavLink to="/" end>
        EL-BORAI
      </NavLink>

      <div>
        {links.map(({ to, label, end }) => (
          <NavLink key={to} to={to} end={end}>
            {label}
          </NavLink>
        ))}

        {isAuthenticated && isCustomer && (
          <NavLink to="/orders">My Orders</NavLink>
        )}

        {isAuthenticated && isMerchant && (
          <NavLink to="/merchant/products">
            My Products
          </NavLink>
        )}

        {isAuthenticated ? (
          <>
            <span>
              {user?.preferred_username ?? user?.name ?? "Account"}
            </span>

            <button type="button" onClick={logout}>
              Logout
            </button>
          </>
        ) : (
          <button type="button" onClick={login}>
            Login
          </button>
        )}
      </div>
    </nav>
  );
}
