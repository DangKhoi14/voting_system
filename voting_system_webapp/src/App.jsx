import { Routes, Route } from "react-router-dom";
import { ThemeProvider } from "./contexts/ThemeContext";
import HomePage from "./pages/HomePage";
import SignInPage from "./pages/SignInPage";
import SignUpPage from "./pages/SignUpPage";
import CreatePollPage from "./pages/CreatePollPage";
import PollPage from "./pages/PollPage";
import ProfilePage from "./pages/ProfilePage";

function App() {
  return (
    <ThemeProvider>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/signin" element={<SignInPage />} />
        <Route path="/signup" element={<SignUpPage />} />
        <Route path="/profile" element={<ProfilePage />} />
        <Route path="/createpoll" element={<CreatePollPage />} />
        <Route path="/poll/:id" element={<PollPage />} />
      </Routes>
    </ThemeProvider>
  );
}

export default App;
