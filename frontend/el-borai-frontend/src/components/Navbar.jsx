
import { NavLink } from 'react-router'

const links = [
  { to: '/', label: 'Home', end: true },
  { to: '/products', label: 'Products' },
  { to: '/cart', label: 'Cart' },
  { to: '/orders', label: 'My Orders' },
]

export default function Navbar() {
  return (
    <nav>
      <NavLink to="/" end>
        EL-BORAI
      </NavLink>

      <div>
        {links.map(({ to, label, end }) => (
          <NavLink
            key={to}
            to={to}
            end={end}
          >
            {label}
          </NavLink>
        ))}
      </div>
    </nav>
  )
}
