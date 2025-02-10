import React, { useEffect, useState } from 'react';
import { useRouter } from 'next/router';
import { getUserById } from '@/Servicios/apiService';
import '../styles/datos.css';
interface UserData {
  id: number;
  name: string;
  username: string;
  email: string;
  phone: string;
  website: string;
  address: {
    street: string;
    suite: string;
    city: string;
    zipcode: string;
  };
  company: {
    name: string;
    catchPhrase: string;
    bs: string;
  };
}

const UserPage = () => {
  const router = useRouter();
  const { id } = router.query;
  const [user, setUser] = useState<UserData | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (id) {
      const fetchUser = async () => {
        setLoading(true);
        setError(null);
        try {
          const userData = await getUserById(Number(id));
          setUser(userData);
        } catch (error) {
          setError('Error fetching user data');
        } finally {
          setLoading(false);
        }
      };

      fetchUser();
    }
  }, [id]);

  return (
    <div className="user-container mx-auto p-4">
      {loading && <div className="loading">Cargando...</div>}
      {error && <div className="error text-red-500">Error: {error}</div>}

      {user && (
        <div className="user-details mt-4">
          <h2 className="text-2xl font-bold mb-4">Detalles del Usuario</h2>
          <p><strong>Nombre:</strong> {user.name}</p>
          <p><strong>Username:</strong> {user.username}</p>
          <p><strong>Email:</strong> {user.email}</p>
          <p><strong>Teléfono:</strong> {user.phone}</p>
          <p><strong>Website:</strong> {user.website}</p>
          <p><strong>Dirección:</strong> {user.address.street}, {user.address.suite}, {user.address.city}, {user.address.zipcode}</p>
          <p><strong>Compañía:</strong> {user.company.name}</p>
          <p><strong>CatchPhrase:</strong> {user.company.catchPhrase}</p>
          <p><strong>BS:</strong> {user.company.bs}</p>
        </div>
      )}
    </div>
  );
};

export default UserPage;