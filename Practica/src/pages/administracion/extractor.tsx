import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/router';
import { getUsers } from '@/Servicios/apiService';
import { v4 as uuidv4 } from 'uuid';
import NavBar from '@/componentes/NavBar';
import '../styles/extractor.css';

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

const Extractor = () => {
  const router = useRouter();
  const [users, setUsers] = useState<UserData[]>([]);
  const [filteredUsers, setFilteredUsers] = useState<UserData[]>([]);
  const [selectedUser, setSelectedUser] = useState<UserData | null>(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const user = localStorage.getItem('user');
    if (!user) {
      router.push('/administracion/login');
    } else {
      const fetchUsers = async () => {
        setLoading(true);
        setError(null);
        try {
          const usersData = await getUsers();
          setUsers(usersData);
          setFilteredUsers(usersData);
        } catch (error) {
          setError('Error fetching user data');
        } finally {
          setLoading(false);
        }
      };

      fetchUsers();
    }
  }, [router]);

  const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
    const term = e.target.value;
    setSearchTerm(term);
    if (term) {
      const filtered = users.filter(user =>
        user.name.toLowerCase().includes(term.toLowerCase())
      );
      setFilteredUsers(filtered);
    } else {
      setFilteredUsers(users);
    }
  };

  const handleSelectUser = (user: UserData) => {
    setSelectedUser(user);
    setSearchTerm('');
    setFilteredUsers([]);
  };

  const handleGenerateQR = () => {
    if (selectedUser) {
      const uniqueId = uuidv4();
      router.push({
        pathname: '/administracion/qr',
        query: { userId: selectedUser.id, uniqueId }
      });
    }
  };

  const handleRefresh = () => {
    setSearchTerm('');
    setFilteredUsers(users);
    setSelectedUser(null);
  };

  return (
    <div>
      <NavBar />
      <div className="custom-container mx-auto p-4">
        <div className="search-container mb-4">
          <input
            type="text"
            value={searchTerm}
            onChange={handleSearch}
            placeholder="Ingrese el nombre"
            className="custom-input mb-4 p-2 border border-gray-300 rounded"
          />
          {searchTerm && (
            <div className="data-container">
              {filteredUsers.length > 0 ? (
                <ul className="user-list">
                  {filteredUsers.map(user => (
                    <li key={user.id} onClick={() => handleSelectUser(user)} className="user-item">
                      {user.name}
                    </li>
                  ))}
                </ul>
              ) : (
                <div className="no-data">No se encontraron datos</div>
              )}
            </div>
          )}
        </div>

        {loading && <div className="loading">Cargando...</div>}
        {error && <div className="error text-red-500">Error: {error}</div>}

        {selectedUser && (
          <div className="user-details mt-4 p-4 border border-gray-300 rounded bg-white shadow-md">
            <h2 className="text-2xl font-bold mb-4">Detalles del Usuario</h2>
            <p><strong>Nombre:</strong> {selectedUser.name}</p>
            <p><strong>Username:</strong> {selectedUser.username}</p>
            <p><strong>Email:</strong> {selectedUser.email}</p>
            <p><strong>Teléfono:</strong> {selectedUser.phone}</p>
            <p><strong>Website:</strong> {selectedUser.website}</p>
            <p><strong>Dirección:</strong> {selectedUser.address.street}, {selectedUser.address.suite}, {selectedUser.address.city}, {selectedUser.address.zipcode}</p>
            <p><strong>Compañía:</strong> {selectedUser.company.name}</p>
            <p><strong>CatchPhrase:</strong> {selectedUser.company.catchPhrase}</p>
            <p><strong>BS:</strong> {selectedUser.company.bs}</p>
          </div>
        )}

        <div className="button-container mt-4">
          <button onClick={handleGenerateQR} className="custom-button bg-green-500 text-white px-4 py-2 rounded">
            Generar QR
          </button>
        </div>
      </div>
    </div>
  );
};

export default Extractor;