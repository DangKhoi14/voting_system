import { FiArrowLeft, FiUser, FiMail, FiLock } from "react-icons/fi";
import { useTheme } from "../context/ThemeContext";


const SignUpPage = ({ onBack, onSignIn }) => {
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
            <h2 className={`text-3xl font-bold ${darkMode ? "text-white" : "text-gray-900"} mb-6 text-center`}>Sign Up</h2>
            <form className="space-y-6">
              <div>
                <label className={`block text-sm font-medium ${darkMode ? "text-gray-300" : "text-gray-700"} mb-2`}>Username</label>
                <div className="relative">
                  <FiUser className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
                  <input
                    type="text"
                    className={`w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${darkMode ? "bg-gray-700 text-white" : "bg-white text-gray-900"}`}
                    placeholder="Enter your username"
                  />
                </div>
              </div>
              <div>
                <label className={`block text-sm font-medium ${darkMode ? "text-gray-300" : "text-gray-700"} mb-2`}>Email</label>
                <div className="relative">
                  <FiMail className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
                  <input
                    type="email"
                    className={`w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${darkMode ? "bg-gray-700 text-white" : "bg-white text-gray-900"}`}
                    placeholder="Enter your email"
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
              <div>
                <label className={`block text-sm font-medium ${darkMode ? "text-gray-300" : "text-gray-700"} mb-2`}>Confirm Password</label>
                <div className="relative">
                  <FiLock className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
                  <input
                    type="password"
                    className={`w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${darkMode ? "bg-gray-700 text-white" : "bg-white text-gray-900"}`}
                    placeholder="Confirm your password"
                  />
                </div>
              </div>
              <button type="submit" className="w-full bg-blue-600 text-white py-2 rounded-lg hover:bg-blue-700 transition-colors">
                Sign Up
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