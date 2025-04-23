import { useContext, useState } from "react";
import { UserContext } from "../contexts/UserContext";
import { FiArrowLeft, FiMail, FiEdit, FiPhone } from "react-icons/fi";
import { useNavigate } from "react-router-dom";
import { useTheme } from "../contexts/ThemeContext";

const ProfilePage = () => {
  const { darkMode } = useTheme();
  const [isEditing, setIsEditing] = useState(false);
  const navigate = useNavigate();
  const { user } = useContext(UserContext) || {}; // Assuming UserContext is defined and provides user data

  const onBack = () => {
    navigate(-1);
  };

  const [profile, setProfile] = useState({
    name: user?.name || "Unknown User",
    email: user?.email || "No email",
    phone: user?.phone || "No phone",
    bio: user?.bio || "No bio available",
    polls: user?.polls || 0,
    participations: user?.participations || 0
  });

  return (
    <div className={`min-h-screen ${darkMode ? "bg-gray-900" : "bg-gray-50"} py-8 px-4 sm:px-6 lg:px-8`}>
      <div className="max-w-4xl mx-auto">
        <button
          onClick={onBack}
          className={`flex items-center ${darkMode ? "text-white" : "text-gray-800"} mb-6 hover:opacity-80`}
        >
          <FiArrowLeft className="mr-2" /> Back to Polls
        </button>
        <div className={`${darkMode ? "bg-gray-800" : "bg-white"} rounded-lg shadow-lg p-8`}>
          <div className="flex justify-between items-start mb-6">
            <div className="flex items-center">
              <div className="w-24 h-24 rounded-full bg-blue-500 flex items-center justify-center text-white text-3xl">
                {profile.name.split(" ").map(n => n[0]).join("")}
              </div>
              <div className="ml-6">
                <h1 className={`text-3xl font-bold ${darkMode ? "text-white" : "text-gray-900"} mb-2`}>{profile.name}</h1>
                <button
                  onClick={() => setIsEditing(!isEditing)}
                  className="flex items-center text-blue-500 hover:text-blue-600"
                >
                  <FiEdit className="mr-2" /> Edit Profile
                </button>
              </div>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-8 mb-8">
            <div>
              <h2 className={`text-xl font-semibold ${darkMode ? "text-white" : "text-gray-900"} mb-4`}>Contact Information</h2>
              <div className="space-y-4">
                <div className="flex items-center">
                  <FiMail className={`mr-3 ${darkMode ? "text-gray-300" : "text-gray-600"}`} />
                  <span className={darkMode ? "text-gray-300" : "text-gray-600"}>{profile.email}</span>
                </div>
                <div className="flex items-center">
                  <FiPhone className={`mr-3 ${darkMode ? "text-gray-300" : "text-gray-600"}`} />
                  <span className={darkMode ? "text-gray-300" : "text-gray-600"}>{profile.phone}</span>
                </div>
              </div>
            </div>
            <div>
              <h2 className={`text-xl font-semibold ${darkMode ? "text-white" : "text-gray-900"} mb-4`}>Statistics</h2>
              <div className="grid grid-cols-2 gap-4">
                <div className={`${darkMode ? "bg-gray-700" : "bg-gray-50"} p-4 rounded-lg text-center`}>
                  <div className={`text-2xl font-bold ${darkMode ? "text-white" : "text-gray-900"}`}>{profile.polls}</div>
                  <div className={darkMode ? "text-gray-300" : "text-gray-600"}>Polls Created</div>
                </div>
                <div className={`${darkMode ? "bg-gray-700" : "bg-gray-50"} p-4 rounded-lg text-center`}>
                  <div className={`text-2xl font-bold ${darkMode ? "text-white" : "text-gray-900"}`}>{profile.participations}</div>
                  <div className={darkMode ? "text-gray-300" : "text-gray-600"}>Participations</div>
                </div>
              </div>
            </div>
          </div>

          <div className="mb-8">
            <h2 className={`text-xl font-semibold ${darkMode ? "text-white" : "text-gray-900"} mb-4`}>Bio</h2>
            <p className={darkMode ? "text-gray-300" : "text-gray-600"}>{profile.bio}</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProfilePage;