import type { NextConfig } from "next";
import dotenv from 'dotenv';

// Cargar las variables de entorno desde el archivo .env
dotenv.config();

const nextConfig: NextConfig = {
  env: {
    API_BASE_URL: process.env.API_BASE_URL,
  },
};

console.log("API_BASE_URL:", process.env.API_BASE_URL); // Verifica el valor de API_BASE_URL

export default nextConfig;