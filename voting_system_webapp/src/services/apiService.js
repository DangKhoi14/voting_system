import axios from "axios";
import API_CONFIG from "../config/apiConfig";

const api = axios.create({
  baseURL: API_CONFIG.BASE_URL,
  timeout: API_CONFIG.TIMEOUT || 10000, // Mặc định 10s nếu không cấu hình
  headers: API_CONFIG.HEADERS || { "Content-Type": "application/json" },
});

// Interceptor: Thêm token vào request nếu có
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Hàm refresh token (cần có API endpoint `/auth/refresh`)
const refreshToken = async () => {
  try {
    const refresh = localStorage.getItem("refreshToken");
    if (!refresh) throw new Error("No refresh token");

    const response = await axios.post(`${API_CONFIG.BASE_URL}/auth/refresh`, {
      refreshToken: refresh,
    });

    // Lưu token mới
    localStorage.setItem("token", response.data.accessToken);
    return response.data.accessToken;
  } catch (error) {
    console.error("Failed to refresh token", error);
    localStorage.removeItem("token");
    localStorage.removeItem("refreshToken");
    return null;
  }
};

// Interceptor: Xử lý lỗi response
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (!error.response) {
      console.error("Network Error:", error.message);
      return Promise.reject({ message: "Không thể kết nối đến server." });
    }

    const { status } = error.response;

    // Nếu lỗi 401, thử refresh token
    if (status === 401) {
      const newToken = await refreshToken();
      if (newToken) {
        error.config.headers.Authorization = `Bearer ${newToken}`;
        return api.request(error.config); // Gửi lại request với token mới
      }
    }

    return Promise.reject(error.response.data || { message: "Có lỗi xảy ra!" });
  }
);

// Hàm GET
export const get = async (url, params = {}) => {
  try {
    const response = await api.get(url, { params });
    return response.data;
  } catch (error) {
    throw error;
  }
};

// Hàm POST
export const post = async (url, data) => {
  try {
    const response = await api.post(url, data);
    return response.data;
  } catch (error) {
    throw error;
  }
};

// Hàm PUT
export const put = async (url, data) => {
  try {
    const response = await api.put(url, data);
    return response.data;
  } catch (error) {
    throw error;
  }
};

// Hàm DELETE
export const del = async (url) => {
  try {
    const response = await api.delete(url);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export default api;
