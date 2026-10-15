
import { Link } from "react-router";

export default function HomePage() {
  return (
    <main>
      <h1>EL-BORAI</h1>
      <p>Your place for tools and building supplies.</p>
      <Link to="/products">Browse Products</Link>
    </main>
  );
}
