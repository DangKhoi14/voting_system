const API_CONFIG = {
    BASE_URL: process.env.REACT_APP_API_BASE_URL || "https://fallback-url.com/api",
    TIMEOUT: Number(process.env.REACT_APP_API_TIMEOUT) || 5000,
    HEADERS: {
      "Content-Type": "application/json",
    },
  };
  
  export default API_CONFIG;