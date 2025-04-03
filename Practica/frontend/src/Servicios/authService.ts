import axios from 'axios';

const API_URL = "http://localhost:5056/api/Auth";

console.log("API_URL:", API_URL); // Verifica el valor de API_URL


export const login = async (email: string, password: string) => {
  try {
    const response = await axios.post(`${API_URL}/login`, {
      email,
      password,
    });
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      if (error.response) {
        if (error.response.status === 401) {
          throw new Error('Credenciales incorrectas');
        } else {
          throw new Error(`Error en la autenticación: ${error.response.data.message || error.response.status}`);
        }
      } else if (error.request) {
        throw new Error('No se recibió respuesta del servidor');
      } else {
        throw new Error(`Error en la solicitud: ${error.message}`);
      }
    } else {
      throw new Error('Error desconocido');
    }
  }
};

export const testConnection = async () => {
  try {
    const response = await axios.get(`${API_URL}/ping`);
    return response.status === 200;
  } catch {
    return false;
  }
};