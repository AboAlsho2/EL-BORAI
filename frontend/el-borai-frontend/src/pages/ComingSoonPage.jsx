
import { Link } from "react-router";

export default function ComingSoonPage({ title }) {
  return (
    <main>
      <h1>{title}</h1>
      <p>This feature is not available yet.</p>
      <Link to="/">Back to Home</Link>
    </main>
  );
}
