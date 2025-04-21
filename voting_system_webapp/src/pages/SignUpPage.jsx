import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FiArrowLeft, FiUser, FiMail, FiLock } from "react-icons/fi";
import { useTheme } from "../context/ThemeContext";
import api from "../services/apiService";

const SignUpPage = ({ onBack, onSignIn }) => {
    const { darkMode } = useTheme();
    const [formData, setFormData] = useState({ username: "", email: "", password: "", confirmPassword: "" });
    const [error, setError] = useState("");
    const [success, setSuccess] = useState(""); // ✅ Thêm state hiển thị thông báo thành công
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
      e.preventDefault();
      setError(""); 
      setSuccess("");
      setLoading(true);
  
      if (formData.password !== formData.confirmPassword) {
          setError("Passwords do not match.");
          setLoading(false);
          return;
      }
  
      try {
          const response = await api.post("accounts/create", {
              username: formData.username,
              email: formData.email,
              password: formData.password,
          });
  
          if (response.status === 201 || response.status === 200) {
              setSuccess("Account created successfully! 🎉");

              const loginResponse = await api.post("Accounts/Login", {
                usernameoremail: formData.email,
                password: formData.password,
              });

              if (loginResponse.status === 200) {
                localStorage.setItem("token", loginResponse.data.accessToken);
                localStorage.setItem("refreshToken", loginResponse.data.refreshToken);
                setSuccess("Sign-in successful! 🎉");
                setTimeout(() => navigate("/"), 1500);
              }
          } else {
              setError(response.data.message || "Sign-up failed.");
          }
      } catch (err) {
          if (err.response?.status === 409) {
              setError("Username already exists.");
          } else {
              setError(err.response?.data?.message || "Sign-up failed.");
          }
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
                    <h2 className={`text-3xl font-bold ${darkMode ? "text-white" : "text-gray-900"} mb-6 text-center`}>
                        Sign Up
                    </h2>
                    <form className="space-y-6" onSubmit={handleSubmit}>
                        <div>
                            <label className={`block text-sm font-medium ${darkMode ? "text-gray-300" : "text-gray-700"} mb-2`}>Username</label>
                            <div className="relative">
                                <FiUser className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
                                <input
                                    type="text"
                                    name="username"
                                    value={formData.username}
                                    onChange={handleChange}
                                    className={`w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${darkMode ? "bg-gray-700 text-white" : "bg-white text-gray-900"}`}
                                    placeholder="Enter your username"
                                    required
                                />
                            </div>
                        </div>
                        <div>
                            <label className={`block text-sm font-medium ${darkMode ? "text-gray-300" : "text-gray-700"} mb-2`}>Email</label>
                            <div className="relative">
                                <FiMail className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
                                <input
                                    type="email"
                                    name="email"
                                    value={formData.email}
                                    onChange={handleChange}
                                    className={`w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${darkMode ? "bg-gray-700 text-white" : "bg-white text-gray-900"}`}
                                    placeholder="Enter your email"
                                    required
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
                                    className={`w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${darkMode ? "bg-gray-700 text-white" : "bg-white text-gray-900"}`}
                                    placeholder="Enter your password"
                                    required
                                />
                            </div>
                        </div>
                        <div>
                            <label className={`block text-sm font-medium ${darkMode ? "text-gray-300" : "text-gray-700"} mb-2`}>Confirm Password</label>
                            <div className="relative">
                                <FiLock className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
                                <input
                                    type="password"
                                    name="confirmPassword"
                                    value={formData.confirmPassword}
                                    onChange={handleChange}
                                    className={`w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${darkMode ? "bg-gray-700 text-white" : "bg-white text-gray-900"}`}
                                    placeholder="Confirm your password"
                                    required
                                />
                            </div>
                        </div>

                        {/* Hiển thị thông báo lỗi (màu đỏ) */}
                        {error && <p className="text-red-500 text-center text-sm">{error}</p>}

                        {/* Hiển thị thông báo thành công (màu xanh) */}
                        {success && <p className="text-green-500 text-center text-sm">{success}</p>}

                        <button
                            type="submit"
                            className="w-full bg-blue-600 text-white py-2 rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-50"
                            disabled={loading}
                        >
                            {loading ? "Signing Up..." : "Sign Up"}
                        </button>
                    </form>
                    <p className={`mt-4 text-center ${darkMode ? "text-gray-300" : "text-gray-600"}`}>
                        Already have an account?{" "}
                        <button onClick={onSignIn} className="text-blue-600 hover:text-blue-700 font-medium">
                            Sign In
                        </button>
                    </p>
                </div>
            </div>
        </div>
    );
};

export default SignUpPage;
