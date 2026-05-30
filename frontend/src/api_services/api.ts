import axios from "axios";
import { readItem } from "../helpers/local_storage";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
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
