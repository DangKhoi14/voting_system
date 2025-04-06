import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom"; // 👈 cần import
import { FiArrowLeft, FiMail, FiLock } from "react-icons/fi";
import { useTheme } from "../context/ThemeContext";
import api from "../services/apiService";

const SignInPage = ({ onBack, onSignUp }) => {
  const { darkMode } = useTheme();
  const [formData, setFormData] = useState({ usernameoremail: "", password: "" });
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate(); // dùng để chuyển trang

  useEffect(() => {
    if (success) {
      navigate("/home"); // chỉ điều hướng nếu đăng nhập thành công
    }
  }, [success, navigate]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");
    setLoading(true);

    try {
      const response = await api.post("Accounts/Login", {
        usernameoremail: formData.usernameoremail,
        password: formData.password,
      });

      if (response.data.statusCode === 200) {
        localStorage.setItem("token", response.data.accessToken);
        localStorage.setItem("refreshToken", response.data.refreshToken);
        setSuccess("Sign-in successful! 🎉");
      } else {
        setError(response.data.message || "Sign-in failed.");
      }
    } catch (err) {
      setError(err.response?.data?.message || "Sign-in failed.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={`min-h-screen ${darkMode ? "bg-gray-900" : "bg-gray-50"} py-8 px-4 sm:px-6 lg:px-8`}>
      <div className="max-w-md mx-auto">
        <button
          onClick={onBack}
          className={`flex items-center ${darkMode ? "text-white" : "text-gray-800"} mb-6 hover:opacity-80`}
        >
          <FiArrowLeft className="mr-2" /> Back to Home
        </button>
        <div className={`${darkMode ? "bg-gray-800" : "bg-white"} rounded-lg shadow-lg p-8`}>
          <h2 className={`text-3xl font-bold ${darkMode ? "text-white" : "text-gray-900"} mb-6 text-center`}>Sign In</h2>

          <form className="space-y-6" onSubmit={handleSubmit}>
            <div>
              <label className={`block text-sm font-medium ${darkMode ? "text-gray-300" : "text-gray-700"} mb-2`}>Email or Username</label>
              <div className="relative">
                <FiMail className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
                <input
                  type="text"
                  name="usernameoremail"
                  value={formData.usernameoremail}
                  onChange={handleChange}
                  className={`w-full pl-10 pr-4 py-2 border rounded-lg focus:ring-2 ${darkMode ? "bg-gray-700 text-white" : "bg-white text-gray-900"}`}
                  placeholder="Enter your email or username"
                />
              </div>
            </div>

            <div>
              <label className={`block text-sm font-medium ${darkMode ? "text-gray-300" : "text-gray-700"} mb-2`}>Password</label>
              <div className="relative">
                <FiLock className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
                <input
                  type="password"
                  name="password"
                  value={formData.password}
                  onChange={handleChange}
                  className={`w-full pl-10 pr-4 py-2 border rounded-lg focus:ring-2 ${darkMode ? "bg-gray-700 text-white" : "bg-white text-gray-900"}`}
                  placeholder="Enter your password"
                />
              </div>
            </div>

            {error && <p className="text-red-500 text-sm text-center">{error}</p>}
            {success && <p className="text-green-500 text-sm text-center">{success}</p>}

            <button type="submit" className="w-full bg-blue-600 text-white py-2 rounded-lg hover:bg-blue-700 transition-colors">
              {loading ? "Signing In..." : "Sign In"}
            </button>
          </form>

          <p className={`mt-4 text-center ${darkMode ? "text-gray-300" : "text-gray-600"}`}>
            Don't have an account?{" "}
            <button onClick={onSignUp} className="text-blue-600 hover:text-blue-700 font-medium">
              Sign Up
            </button>
          </p>
        </div>
      </div>
    </div>
  );
};

export default SignInPage;
