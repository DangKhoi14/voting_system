import { useContext } from "react";
import { UserContext } from "../contexts/UserContext";
import { FiSun, FiMoon } from "react-icons/fi";
import { useNavigate } from "react-router-dom";
import Icon from "../assets/icon.png";

const Header = ({ darkMode, setDarkMode }) => {
  const { user, checked } = useContext(UserContext);
  const navigate = useNavigate();

  return (
    <div className="absolute right-0 top-0 space-x-4 flex items-center">
      <button onClick={() => setDarkMode(!darkMode)} className={`p-2 rounded-full ${darkMode ? "bg-gray-800 text-yellow-300" : "bg-gray-200 text-gray-800"}`}>
        {darkMode ? <FiMoon size={20} /> : <FiSun size={20} />}
      </button>

      {checked && user ? (
        <img
          src={user.avatarUrl || "/default-avatar.png"}
          alt="avatar"
          className="w-8 h-8 rounded-full cursor-pointer"
          onClick={() => navigate("/profile")}
        />
      ) : (
        <>
          <button onClick={() => navigate("/signin")} className={`px-4 py-2 ${darkMode ? "text-blue-400" : "text-blue-600"} hover:text-blue-700 font-medium`}>
            Sign In
          </button>
          <button onClick={() => navigate("/signup")} className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-200 font-medium">
            Sign Up
          </button>
        </>
      )}
    </div>
  );
};

export default Header;
