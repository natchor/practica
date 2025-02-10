import React, { useEffect, useState } from 'react';
import { useRouter } from 'next/router';
import { getUserById } from '@/Servicios/apiService';
import '../styles/web.css';

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
    <div className="user-container">
      {loading && <div className="loading">Cargando...</div>}
      {error && <div className="error">Error: {error}</div>}

      {user && (
        <div className="user-details">
          <h2>Detalles del Usuario</h2>
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