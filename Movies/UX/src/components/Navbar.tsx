import { Link, useLocation } from "react-router-dom";

const Navbar = () => {
    const { pathname } = useLocation();

    const isActive = (path: string) =>
        pathname === path ? "nav-link active" : "nav-link";

    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light px-3">
            <Link className="navbar-brand" to="/">Movie Store</Link>
            <div className="navbar-nav">
                <Link className={isActive("/")} to="/">Home</Link>  
            </div>
        </nav>
    );
};

export default Navbar;
