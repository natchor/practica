import axios from 'axios';

const API_URL = "http://localhost:5056/api/Ficha/all";
console.log("API_URL:", API_URL); // Verifica el valor de API_URL
const apiClient = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

interface Ficha {
  id: number;
  rutCon: string;
  nombres?: string;
  apellidoMaterno?: string;
  apellidoPaterno?: string;
  fechaNacimiento: string;
  alergias?: string;
  medicamentos?: string;
  enfermedades?: string;
  mutualidad?: string;
  grupoSanguineo?: string;
  factorRH?: string;
  obs?: string;
  nombreCont?: string;
  telefono?: string;
  direccionCont?: string;
  nombreCont2?: string;
  telefono2?: string;
  direccionCont2?: string;
  nombreCont3?: string;
  telefono3?: string;
  direccionCont3?: string;
}

const mapFicha = (ficha: Partial<Ficha>): Ficha => ({
  id: ficha.id || 0,
  rutCon: ficha.rutCon || '',
  nombres: ficha.nombres || '',
  apellidoMaterno: ficha.apellidoMaterno || '',
  apellidoPaterno: ficha.apellidoPaterno || '',
  fechaNacimiento: ficha.fechaNacimiento || '',
  alergias: ficha.alergias || '',
  medicamentos: ficha.medicamentos || '',
  enfermedades: ficha.enfermedades || '',
  mutualidad: ficha.mutualidad || '',
  grupoSanguineo: ficha.grupoSanguineo || '',
  factorRH: ficha.factorRH || '',
  obs: ficha.obs || '',
  nombreCont: ficha.nombreCont || '',
  telefono: ficha.telefono || '',
  direccionCont: ficha.direccionCont || '',
  nombreCont2: ficha.nombreCont || '',
  telefono2: ficha.telefono || '',
  direccionCont2: ficha.direccionCont || '',
  nombreCont3: ficha.nombreCont || '',
  telefono3: ficha.telefono || '',
  direccionCont3: ficha.direccionCont || '',
});

export const getAllFichas = async (): Promise<Ficha[]> => {
  try {
    const response = await apiClient.get<Ficha[]>('/');
    return response.data.map(mapFicha);
  } catch (error) {
    console.error('Error al obtener todas las fichas:', error);
    throw new Error('Error al obtener todas las fichas');
  }
};

