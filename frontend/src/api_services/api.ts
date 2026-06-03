import axios from "axios";
import { readItem } from "../helpers/local_storage";

const getBaseUrl = () => {
  if (window.location.hostname.includes("ngrok")) {
    return "https://take-submarine-collapse.ngrok-free.dev";
  }
  return import.meta.env.VITE_API_URL;
};

const api = axios.create({
  baseURL: getBaseUrl(),
});

// Automatski dodaje token u svaki request
api.interceptors.request.use((config) => {
  const token = readItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
