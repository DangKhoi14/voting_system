import { useEffect, useState } from "react";
import { useTheme } from "../context/ThemeContext";
import PollCard from "../components/PollCard";
import { FiSearch, FiMoon, FiSun } from "react-icons/fi";

const HomePage = () => {
  const { darkMode, setDarkMode } = useTheme();
  const [searchTerm, setSearchTerm] = useState("");
  const [polls, setPolls] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchPolls = async () => {
      try {
        const dummyPolls = [
          { id: 1, title: "Best Programming Language 2024", author: "John Doe", status: "ongoing", startDate: "2024-01-01", endDate: "2024-02-01", participationCount: 1234 },
          { id: 2, title: "Company Logo Selection", author: "Jane Smith", status: "completed", startDate: "2023-12-01", endDate: "2023-12-31", participationCount: 567 },
          { id: 3, title: "Annual Employee Satisfaction Survey", author: "Mike Johnson", status: "ongoing", startDate: "2024-01-15", endDate: "2024-02-15", participationCount: 890 },
        ];
        setPolls(dummyPolls);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching polls:", error);
        setLoading(false);
      }
    };
    fetchPolls();
  }, []);

  const filteredPolls = polls.filter((poll) => poll.title.toLowerCase().includes(searchTerm.toLowerCase()));

  return (
    <div className={`min-h-screen ${darkMode ? "bg-gray-900" : "bg-gray-50"} py-8 px-4 sm:px-6 lg:px-8`}>
      <div className="max-w-7xl mx-auto relative">
        <div className="absolute right-0 top-0 space-x-4 flex items-center">
          <button onClick={() => setDarkMode(!darkMode)} className={`p-2 rounded-full ${darkMode ? "bg-gray-800 text-yellow-300" : "bg-gray-200 text-gray-800"}`}>
            {darkMode ? <FiSun size={20} /> : <FiMoon size={20} />}
          </button>
          <button className={`px-4 py-2 ${darkMode ? "text-blue-400" : "text-blue-600"} hover:text-blue-700 font-medium`}>Sign In</button>
          <button className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-200 font-medium">Sign Up</button>
        </div>
        <div className="text-center mb-8 pt-16">
          <h1 className={`text-4xl font-bold ${darkMode ? "text-white" : "text-gray-900"} mb-4`}>Online Voting System</h1>
          <p className={`text-xl ${darkMode ? "text-gray-300" : "text-gray-600"}`}>Participate in active polls or check results</p>
        </div>
        <div className="relative max-w-xl mx-auto mb-8">
          <FiSearch className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
          <input type="text" placeholder="Search polls..." className={`w-full pl-10 pr-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${darkMode ? "bg-gray-800 text-white" : "bg-white text-gray-900"}`} value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
        </div>
        {loading ? (
          <p className="text-center text-gray-500">Loading...</p>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {filteredPolls.map((poll) => (
              <PollCard key={poll.id} poll={poll} />
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default HomePage;
