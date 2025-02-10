import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'https://localhost:5001', // Asegúrate de que esta URL coincida con la URL de tu API
  headers: {
    'Content-Type': 'application/json',
  },
});

export const getUser = async (searchTerm) => {
  try {
    const response = await apiClient.get('/JsonPlaceholder/users', {
      params: { searchTerm }
    });
    return response.data;
  } catch (error) {
    console.error('Error fetching users data:', error);
    throw error;
  }
};