import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  server: {
    allowedHosts: ["take-submarine-collapse.ngrok-free.dev"],
    proxy: {
      "/api": {
        target: "http://localhost:8635",
        changeOrigin: true,
        secure: false,
      },
    },
  },
});
