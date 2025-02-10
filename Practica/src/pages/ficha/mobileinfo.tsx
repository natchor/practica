import React, { useEffect, useState } from 'react';
import { getUsers } from '@/Servicios/apiService';
import './styles/mobileExtractor.css';

interface UserData {
  nombre: string;
  email: string;
}

const MobileExtractor = () => {
  const [data, setData] = useState<UserData | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      setError(null);
      try {
        const userData = await getUsers(); // Puedes ajustar el parámetro según sea necesario
        setData(userData);
      } catch (error) {
        setError('Error fetching user data');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  return (
    <div className="mobile-container mx-auto p-4">
      {loading && <div className="loading">Cargando...</div>}
      {error && <div className="error text-red-500">Error: {error}</div>}

      {data && (
        <div className="data-container">
          <h2 className="text-2xl font-bold mb-4">Datos de la Persona</h2>
          <div className="data-item">
            <strong>Nombre:</strong> {data.nombre}
          </div>
          <div className="data-item">
            <strong>Email:</strong> {data.email}
          </div>
        </div>
      )}
    </div>
  );
};

export default MobileExtractor;