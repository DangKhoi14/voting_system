
import { useEffect, useState } from "react";
import { FiArrowLeft, FiCalendar, FiUsers, FiMoon, FiSun } from "react-icons/fi";
import { format, isFuture, isPast } from "date-fns";
import { useTheme } from "../context/ThemeContext";
import api from "../services/apiService";

const PollPage = ({ poll, onBack }) => {
    const { darkMode, setDarkMode } = useTheme();
    const [selectedOption, setSelectedOption] = useState(null);
    const [options, setOptions] = useState([]);
    
    let statusColor, statusText;  
    if (isFuture(new Date(poll.startDate))) {
      statusColor = "bg-yellow-500";
      statusText = "Upcoming";
    } else if (isPast(new Date(poll.endDate))) {
      statusColor = "bg-red-500";
      statusText = "Completed";
    } else {
      statusColor = "bg-green-500";
      statusText = "Ongoing";
    }

    useEffect(() => {
      const fetchOptions = async () => {
          try {
            const response = await api.get("/Options/GetOptionsByPollId?PollId=" + poll.id);
            if (response.data && response.data.statusCode === 200) {
              const realOptions = response.data.data.map((option) => ({
                id: option.optionId,
                text: option.optionText,
                votes: option.voteCount,
              }));
              setOptions(realOptions);
            }
          } catch (error) {
              console.error("Error fetching poll data:", error);
          }
      };
      fetchOptions();
    }, [poll.id]);
  
    return (
      <div className={`min-h-screen ${darkMode ? "bg-gray-900" : "bg-gray-50"} py-8 px-4 sm:px-6 lg:px-8`}>
        <div className="max-w-7xl mx-auto relative">
            <div className="absolute right-0 top-0 space-x-4 flex items-center">
                <button onClick={() => setDarkMode(!darkMode)} className={`p-2 rounded-full ${darkMode ? "bg-gray-800 text-yellow-300" : "bg-gray-200 text-gray-800"}`}>
                {darkMode ? <FiMoon size={20} /> : <FiSun size={20} />}
                </button>
                <button className={`px-4 py-2 ${darkMode ? "text-blue-400" : "text-blue-600"} hover:text-blue-700 font-medium`}>Sign In</button>
                <button className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-200 font-medium">Sign Up</button>
            </div>
        </div>

        <div className="max-w-4xl mx-auto">
          <button
            onClick={onBack}
            className={`flex items-center ${darkMode ? "text-white" : "text-gray-800"} mb-6 hover:opacity-80`}
          >
            <FiArrowLeft className="mr-2" /> Back to Polls
          </button>
          <div className="max-w-4xl mx-auto">
            <div className="flex justify-between items-start mb-6">
              <div>
                <h1 className={`text-3xl font-bold ${darkMode ? "text-white" : "text-gray-900"} mb-2`}>{poll.title}</h1>
                <p className={`${darkMode ? "text-gray-300" : "text-gray-600"}`}>Poll ID: {poll.id}</p>
                <p className={`${darkMode ? "text-gray-300" : "text-gray-600"}`}>Created by: {poll.author} (User ID: {poll.authorid})</p>
              </div>
              <span className={`${statusColor} text-white px-3 py-1 rounded-full text-sm`}>{statusText}</span>
            </div>
            
            <div className={`${darkMode ? "text-gray-300" : "text-gray-700"} mb-8`}>
              <h2 className={`text-xl font-semibold ${darkMode ? "text-white" : "text-gray-900"} mb-3`}>Description</h2>
              <p>{poll.description || "No description provided."}</p>
            </div>
  
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
              <div className={`${darkMode ? "bg-gray-700" : "bg-gray-50"} p-4 rounded-lg`}>
                <h3 className={`text-lg font-semibold ${darkMode ? "text-white" : "text-gray-900"} mb-2`}>Timeline</h3>
                <div className="space-y-3">
                  <div className={`flex items-center ${darkMode ? "text-gray-300" : "text-gray-600"}`}>
                    <FiCalendar className="mr-2" />
                    <span>Start: {format(new Date(poll.startDate), "MMM dd, yyyy HH:mm")}</span>
                  </div>
                  <div className={`flex items-center ${darkMode ? "text-gray-300" : "text-gray-600"}`}>
                    <FiCalendar className="mr-2" />
                    <span>End: {format(new Date(poll.endDate), "MMM dd, yyyy HH:mm")}</span>
                  </div>
                </div>
              </div>
              <div className={`${darkMode ? "bg-gray-700" : "bg-gray-50"} p-4 rounded-lg`}>
                <h3 className={`text-lg font-semibold ${darkMode ? "text-white" : "text-gray-900"} mb-2`}>Participation</h3>
                <div className={`flex items-center ${darkMode ? "text-gray-300" : "text-gray-600"}`}>
                  <FiUsers className="mr-2" />
                  <span>{poll.participationCount} participants</span>
                </div>
              </div>
            </div>
  
            <div className={`${darkMode ? "bg-gray-800" : "bg-white"} rounded-lg shadow-lg p-8 mt-6`}>
              <h2 className={`text-xl font-semibold ${darkMode ? "text-white" : "text-gray-900"} mb-4`}>Vote Options</h2>
              <div className="space-y-4">
                {options.map((option) => (
                  <div 
                    key={option.id}
                    onClick={() => setSelectedOption(option.id)}
                    className={`${darkMode ? "bg-gray-700" : "bg-gray-50"} 
                      ${selectedOption === option.id ? "border-2 border-blue-500" : "border border-gray-200"}
                      p-4 rounded-lg cursor-pointer hover:shadow-md transition-all duration-200`}
                  >
                    <div className="flex justify-between items-center">
                      <div className="flex items-center">
                        <div className={`w-5 h-5 rounded-full border-2 ${selectedOption === option.id ? "border-blue-500" : "border-gray-400"} mr-3
                          ${selectedOption === option.id ? "bg-blue-500" : "bg-transparent"}`}></div>
                        <span className={`${darkMode ? "text-white" : "text-gray-900"}`}>{option.text}</span>
                      </div>
                      <span className={`${darkMode ? "text-gray-400" : "text-gray-600"} text-sm`}>{option.votes} votes</span>
                    </div>
                  </div>
                ))}
              </div>
  
              {poll.status === "ongoing" && (
                <button 
                  className={`w-full mt-6 py-3 rounded-lg transition-colors
                    ${selectedOption 
                      ? "bg-blue-600 hover:bg-blue-700 text-white" 
                      : "bg-gray-300 cursor-not-allowed text-gray-500"}`}
                  disabled={!selectedOption}
                >
                  Submit Vote
                </button>
              )}
            </div>
          </div>
        </div>
      </div>
    );
  };

export default PollPage;