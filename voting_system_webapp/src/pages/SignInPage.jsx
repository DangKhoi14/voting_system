import { FiArrowLeft, FiMail, FiLock } from "react-icons/fi";
import { useTheme } from "../context/ThemeContext";


const SignInPage = ({ onBack, onSignUp }) => {
    const { darkMode } = useTheme();

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
            <form className="space-y-6">
              <div>
                <label className={`block text-sm font-medium ${darkMode ? "text-gray-300" : "text-gray-700"} mb-2`}>Email or Username</label>
                <div className="relative">
                  <FiMail className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
                  <input
                    type="emailorusername"
                    className={`w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${darkMode ? "bg-gray-700 text-white" : "bg-white text-gray-900"}`}
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
                    className={`w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${darkMode ? "bg-gray-700 text-white" : "bg-white text-gray-900"}`}
                    placeholder="Enter your password"
                  />
                </div>
              </div>
              <button type="submit" className="w-full bg-blue-600 text-white py-2 rounded-lg hover:bg-blue-700 transition-colors">
                Sign In
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