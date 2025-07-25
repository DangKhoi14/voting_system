import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import { UserProvider } from "./contexts/UserContext";
import './styles/index.css';
import App from './App';
import reportWebVitals from './utils/reportWebVitals';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
      <BrowserRouter>
    <UserProvider>
        <App />
    </UserProvider>
      </BrowserRouter>
  </React.StrictMode>
);


reportWebVitals();
