import { createContext, useState, useEffect } from "react";
import api from "../services/apiService";

export const UserContext = createContext();

export const UserProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [checked, setChecked] = useState(false);

  useEffect(() => {
    const fetchUser = async () => {
      const storedUser = localStorage.getItem("user");
      const token = localStorage.getItem("token");

      if (storedUser) {
        setUser(JSON.parse(storedUser));
        setChecked(true);
        return;
      }

      if (token) {
        try {
          const res = await api.get("/Accounts/Me");
          setUser(res.data);
          localStorage.setItem("user", JSON.stringify(res.data)); // lưu lại
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