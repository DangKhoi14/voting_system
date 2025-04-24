import { useContext } from "react";
import { UserContext } from "../contexts/UserContext";
import { FiSun, FiMoon } from "react-icons/fi";
import { useNavigate } from "react-router-dom";
import defaultAvatar from "../assets/default-avatar.jpg";

const Header = ({ darkMode, setDarkMode }) => {
  // const context = useContext(UserContext);
  // const user = context?.user ?? null;
  // const checked = context?.checked ?? false;
  const { user = null, checked = false } = useContext(UserContext) || {};
  const navigate = useNavigate();

  const getAvatarUrl = () =>
    user?.profilePictureUrl || defaultAvatar;

  return (
    <div className="absolute right-0 top-0 space-x-4 flex items-center">
      <button onClick={() => setDarkMode(!darkMode)} className={`p-2 rounded-full ${darkMode ? "bg-gray-800 text-yellow-300" : "bg-gray-200 text-gray-800"}`}>
        {darkMode ? <FiMoon size={20} /> : <FiSun size={20} />}
      </button>

      {!checked ? null : user ? (
        <img
          src={getAvatarUrl()}
          onError={(e) => {
            e.target.onerror = null;
            e.target.src = defaultAvatar;
          }} // Fallback to default avatar if the image fails to load
          alt="avatar"
          className="w-9 h-9 rounded-full cursor-pointer"
          onClick={() => navigate("/profile")}
        />
      ) : (
        <>
          <button onClick={() => navigate("/signin")} 
            className={`px-4 py-2 ${darkMode ? "text-blue-400" : "text-blue-600"} hover:text-blue-700 font-medium`}>
              Sign In
          </button>
          <button onClick={() => navigate("/signup")} 
            className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-200 font-medium">
              Sign Up
          </button>
        </>
      )}
    </div>
  );
};

export default Header;
