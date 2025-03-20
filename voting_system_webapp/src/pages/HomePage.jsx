import { useEffect, useState } from "react";
import { useTheme } from "../context/ThemeContext";
import PollCard from "../components/PollCard";
import PollPage from "./PollPage";
import SignInPage from "./SignInPage";
import SignUpPage from "./SignUpPage";
import CreatePollPage from "./CreatePollPage";
import { FiSearch, FiMoon, FiSun, FiPlus } from "react-icons/fi";
import api from "../services/apiService";
import Icon from "../assets/logo.svg";


const HomePage = () => {
  const { darkMode, setDarkMode } = useTheme();
  const [searchTerm, setSearchTerm] = useState("");
  const [polls, setPolls] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedPoll, setSelectedPoll] = useState(null);
  const [page, setPage] = useState("home");

  useEffect(() => {
    const fetchPolls = async () => {
      try {
        const response = await api.get("/Polls/GetAll");
        if (response.data && response.data.statusCode === 200) {
          const realPolls = response.data.data.map((poll) => ({
            id: poll.pollId,
            title: poll.title,
            description: poll.description,
            authorid: poll.userId,
            author: poll.userName,
            status: poll.isActive,
            startDate: poll.startTime,
            endDate: poll.endTime,
            participationCount: poll.participantCount,
          }));
          setPolls(realPolls);
        }
      } catch (error) {
        console.error("Error fetching polls:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchPolls();
  }, []);

  const filteredPolls = polls.filter((poll) => poll.title.toLowerCase().includes(searchTerm.toLowerCase()));

  if (page === "signin") {
    return <SignInPage onBack={() => setPage("home")} onSignUp={() => setPage("signup")} />;
  }

  if (page === "signup") {
    return <SignUpPage onBack={() => setPage("home")} onSignIn={() => setPage("signin")} />;
  }

  if (page === "create") {
    return <CreatePollPage onBack={() => setPage("home")} />;
  }

  if (selectedPoll) {
    return <PollPage poll={selectedPoll} onBack={() => setSelectedPoll(null)} />;
  }

  return (
    <div className={`min-h-screen ${darkMode ? "bg-gray-900" : "bg-gray-50"} py-8 px-4 sm:px-6 lg:px-8`}>
      <div className="max-w-7xl mx-auto relative">
        {/* Header and logo */}
        <div className="absolute left-8 top-2 flex items-center space-x-4">
          <img src={Icon} alt="Website Icon" className="w-48 h-48" /> {/* Biểu tượng lớn hơn */}
        </div>
        
        {/* Dark mode, Sign in, Sign up */}
        <div className="absolute right-0 top-0 space-x-4 flex items-center">
          <button onClick={() => setDarkMode(!darkMode)} className={`p-2 rounded-full ${darkMode ? "bg-gray-800 text-yellow-300" : "bg-gray-200 text-gray-800"}`}>
            {darkMode ? <FiMoon size={20} /> : <FiSun size={20} />}
          </button>
          <button 
            onClick={() => setPage("signin")} 
            className={`px-4 py-2 ${darkMode ? "text-blue-400" : "text-blue-600"} hover:text-blue-700 font-medium`}
          >
            Sign In
          </button>
          <button 
            onClick={() => setPage("signup")} 
            className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-200 font-medium"
          >
            Sign Up
          </button>        </div>
        
        {/* Main title */}
        <div className="text-center mb-8 pt-16">
          {/* <h1 className={`text-4xl font-bold ${darkMode ? "text-white" : "text-gray-900"} mb-4`}>Online Voting System</h1> */}
          <h1 className={`text-4xl font-bold mb-4 ${darkMode ? "text-white" : "text-gray-900"}`}>
            <span className="text-red-500 text-5xl">O</span>nline 
            <span className="text-yellow-500 text-5xl"> V</span>oting 
            <span className="text-green-500 text-5xl"> S</span>ystem
          </h1>
          <p className={`text-xl ${darkMode ? "text-gray-300" : "text-gray-600"}`}>Participate in active polls or check results</p>
        </div>
        
        {/* Search bar */}
        <div className="relative max-w-xl mx-auto mb-8 flex items-center">
          <FiSearch className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
          <input type="text" placeholder="Search polls..." className={`w-full pl-10 pr-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${darkMode ? "bg-gray-800 text-white" : "bg-white text-gray-900"}`} value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
          <button className="ml-3 p-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-200" 
                  title="Create New Poll"
                  onClick={() => setPage("create")}>
            <FiPlus size={24} />
          </button>
        </div>
        
        {/* Polls */}
        {loading ? (
          <p className="text-center text-gray-500">Loading...</p>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {filteredPolls.map((poll) => (
              <PollCard key={poll.id} poll={poll} onClick={setSelectedPoll}/>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default HomePage;
