import { useState } from "react";
import { useTheme } from "../contexts/ThemeContext";
import { FiArrowLeft } from "react-icons/fi";
import { useNavigate } from "react-router-dom";

const CreatePollPage = ({ onBack }) => {
  const { darkMode } = useTheme();
  const navigate = useNavigate();

  const [pollData, setPollData] = useState({
    title: "",
    description: "",
    startDate: "",
    endDate: ""
  });

  const onBackHome = () => {
    navigate("/");
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log("Poll created:", pollData);
    onBack();
  };

  return (
    <div className={`min-h-screen ${darkMode ? "bg-gray-900" : "bg-gray-50"} py-8 px-4 sm:px-6 lg:px-8`}>
      <div className="max-w-4xl mx-auto">
        <button
          onClick={onBackHome}
          className={`flex items-center ${darkMode ? "text-white" : "text-gray-800"} mb-6 hover:opacity-80`}
        >
          <FiArrowLeft className="mr-2" /> Back to Polls
        </button>
        <div className={`${darkMode ? "bg-gray-800" : "bg-white"} rounded-lg shadow-lg p-8`}>
          <h1 className={`text-3xl font-bold ${darkMode ? "text-white" : "text-gray-900"} mb-6`}>Create New Poll</h1>
          <form onSubmit={handleSubmit} className="space-y-6">
            <div>
              <label className={`block text-sm font-medium ${darkMode ? "text-gray-300" : "text-gray-700"} mb-2`}>Title</label>
              <input
                type="text"
                required
                className={`w-full px-4 py-2 rounded-lg border ${darkMode ? "bg-gray-700 border-gray-600 text-white" : "bg-white border-gray-300"}`}
                value={pollData.title}
                onChange={(e) => setPollData({ ...pollData, title: e.target.value })}
              />
            </div>
            <div>
              <label className={`block text-sm font-medium ${darkMode ? "text-gray-300" : "text-gray-700"} mb-2`}>Description</label>
              <textarea
                required
                className={`w-full px-4 py-2 rounded-lg border ${darkMode ? "bg-gray-700 border-gray-600 text-white" : "bg-white border-gray-300"}`}
                rows="4"
                value={pollData.description}
                onChange={(e) => setPollData({ ...pollData, description: e.target.value })}
              />
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label className={`block text-sm font-medium ${darkMode ? "text-gray-300" : "text-gray-700"} mb-2`}>Start Date</label>
                <input
                  type="datetime-local"
                  required
                  className={`w-full px-4 py-2 rounded-lg border ${darkMode ? "bg-gray-700 border-gray-600 text-white" : "bg-white border-gray-300"}`}
                  value={pollData.startDate}
                  onChange={(e) => setPollData({ ...pollData, startDate: e.target.value })}
                />
              </div>
              <div>
                <label className={`block text-sm font-medium ${darkMode ? "text-gray-300" : "text-gray-700"} mb-2`}>End Date</label>
                <input
                  type="datetime-local"
                  required
                  className={`w-full px-4 py-2 rounded-lg border ${darkMode ? "bg-gray-700 border-gray-600 text-white" : "bg-white border-gray-300"}`}
                  value={pollData.endDate}
                  onChange={(e) => setPollData({ ...pollData, endDate: e.target.value })}
                />
              </div>
            </div>
            <button
              type="submit"
              className="w-full bg-blue-600 text-white py-3 rounded-lg hover:bg-blue-700 transition-colors"
            >
              Create Poll
            </button>
          </form>
        </div>
      </div>
    </div>
  );
};


export default CreatePollPage;