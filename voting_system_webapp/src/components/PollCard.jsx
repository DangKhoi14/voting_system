import { useTheme } from "../context/ThemeContext";
import { FiCalendar, FiUsers } from "react-icons/fi";
import { format } from "date-fns";

const PollCard = ({ poll }) => {
  const { darkMode } = useTheme();
  const statusColor = poll.status === "ongoing" ? "bg-green-500" : "bg-red-500";

  return (
    <div className={`${darkMode ? "bg-gray-800 text-white" : "bg-white"} rounded-lg shadow-md p-6 transition-transform hover:transform hover:scale-105`}>
      <div className="flex justify-between items-start mb-4">
        <div>
          <h3 className={`text-xl font-semibold ${darkMode ? "text-white" : "text-gray-800"}`}>{poll.title}</h3>
          <p className={`text-sm ${darkMode ? "text-gray-300" : "text-gray-600"} mt-1`}>By {poll.author}</p>
        </div>
        <span className={`${statusColor} text-white px-3 py-1 rounded-full text-sm`}>{poll.status}</span>
      </div>
      <div className="space-y-2">
        <div className={`flex items-center ${darkMode ? "text-gray-300" : "text-gray-600"}`}>
          <FiCalendar className="mr-2" />
          <span>Start: {format(new Date(poll.startDate), "MMM dd, yyyy")}</span>
        </div>
        <div className={`flex items-center ${darkMode ? "text-gray-300" : "text-gray-600"}`}>
          <FiCalendar className="mr-2" />
          <span>End: {format(new Date(poll.endDate), "MMM dd, yyyy")}</span>
        </div>
        <div className={`flex items-center ${darkMode ? "text-gray-300" : "text-gray-600"}`}>
          <FiUsers className="mr-2" />
          <span>{poll.participationCount} participants</span>
        </div>
      </div>
      <button className="mt-4 w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition-colors">View Details</button>
    </div>
  );
};

export default PollCard;
