import React, { useState, useEffect } from "react";
import { FiSearch, FiCalendar, FiUsers, FiCheckCircle } from "react-icons/fi";
import { format } from "date-fns";

const PollCard = ({ poll }) => {
  const statusColor = poll.status === "ongoing" ? "bg-green-500" : "bg-red-500";

  return (
    <div className="bg-white rounded-lg shadow-md p-6 transition-transform hover:transform hover:scale-105">
      <div className="flex justify-between items-start mb-4">
        <h3 className="text-xl font-semibold text-gray-800">{poll.title}</h3>
        <span className={`${statusColor} text-white px-3 py-1 rounded-full text-sm`}>
          {poll.status}
        </span>
      </div>
      <div className="space-y-2">
        <div className="flex items-center text-gray-600">
          <FiCalendar className="mr-2" />
          <span>Start: {format(new Date(poll.startDate), "MMM dd, yyyy")}</span>
        </div>
        <div className="flex items-center text-gray-600">
          <FiCalendar className="mr-2" />
          <span>End: {format(new Date(poll.endDate), "MMM dd, yyyy")}</span>
        </div>
        <div className="flex items-center text-gray-600">
          <FiUsers className="mr-2" />
          <span>{poll.participationCount} participants</span>
        </div>
      </div>
      <button className="mt-4 w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition-colors">
        View Details
      </button>
    </div>
  );
};

const HomePage = () => {
  const [searchTerm, setSearchTerm] = useState("");
  const [polls, setPolls] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchPolls = async () => {
      try {
        const dummyPolls = [
          {
            id: 1,
            title: "Best Programming Language 2024",
            status: "ongoing",
            startDate: "2024-01-01",
            endDate: "2024-02-01",
            participationCount: 1234
          },
          {  
            id: 2,
            title: "Company Logo Selection",
            status: "completed",
            startDate: "2023-12-01",
            endDate: "2023-12-31",
            participationCount: 567
          },
          {
            id: 3,
            title: "Annual Employee Satisfaction Survey",
            status: "ongoing",
            startDate: "2024-01-15",
            endDate: "2024-02-15",
            participationCount: 890
          }
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

  const filteredPolls = polls.filter(poll =>
    poll.title.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="min-h-screen bg-gray-50 py-8 px-4 sm:px-6 lg:px-8">
      <div className="max-w-7xl mx-auto">
        <div className="text-center mb-8">
          <h1 className="text-4xl font-bold text-gray-900 mb-4">Online Voting System</h1>
          <p className="text-xl text-gray-600">Participate in active polls or check results</p>
        </div>

        <div className="relative max-w-xl mx-auto mb-8">
          <FiSearch className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
          <input
            type="text"
            placeholder="Search polls..."
            className="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>

        {loading ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {[1, 2, 3].map((n) => (
              <div key={n} className="animate-pulse bg-white rounded-lg shadow-md p-6">
                <div className="h-4 bg-gray-200 rounded w-3/4 mb-4"></div>
                <div className="space-y-3">
                  <div className="h-4 bg-gray-200 rounded"></div>
                  <div className="h-4 bg-gray-200 rounded"></div>
                  <div className="h-4 bg-gray-200 rounded"></div>
                </div>
              </div>
            ))}
          </div>
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

// This is a React.js frontend application using:
// - React for UI components and state management
// - Tailwind CSS for styling
// - React Icons for iconography
// - date-fns for date formatting

export default HomePage;