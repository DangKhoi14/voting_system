import { createContext, useState, useEffect } from "react";
import api from "../services/apiService";

export const UserContext = createContext();

export const UserProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [checked, setChecked] = useState(false);

  const refreshUser = async () => {
    console.log("refreshUser called");
    const token = localStorage.getItem("accessToken");

    if (!token) {
      return;
    }

    try {
      const res = await api.get("/Accounts/Me");
      if (res.data?.data) {
        setUser(res.data.data);
        localStorage.setItem("user", JSON.stringify(res.data.data));
        console.log("User refreshed:", res.data.data);
      } else {
        setUser(null);
      }
    } catch (err) {
      console.error("refreshUser error:", err.response?.data || err.message);
      setUser(null);
    } finally {
      setChecked(true);
    }
  };

  // Gọi khi app load
  useEffect(() => {
    refreshUser();
  }, []);

  useEffect(() => {
    const fetchUser = async () => {
      const storedUser = localStorage.getItem("user");
      const token = localStorage.getItem("accessToken");

      if (storedUser) {
        setUser(JSON.parse(storedUser));
        setChecked(true);
        return;
      }

      if (token) {
        try {
          const res = await api.get("/Accounts/Me");
          setUser(res.data.data);
          localStorage.setItem("user", JSON.stringify(res.data.data));
        } catch (err) {
          setUser(null);
        }
      }

      setChecked(true);
    };

    fetchUser();
  }, []);

  return (
    <UserContext.Provider value={{ user, setUser, checked, setChecked }}>
      {children}
    </UserContext.Provider>
  );
};