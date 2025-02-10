import React, { useState } from 'react';
import { useRouter } from 'next/router';
import { getUsers } from '@/Servicios/apiService';
import '../styles/login.css';

const LoginPage = () => {
  const router = useRouter();
  const [email, setEmail] = useState('');
  const [error, setError] = useState<string | null>(null);

  const handleLogin = async () => {
    try {
      const users = await getUsers();
      const user = users.find((user: { email: string }) => user.email === email);
      if (user) {
        // Save user info to localStorage or context
        localStorage.setItem('user', JSON.stringify(user));
        router.push('/administracion/extractor');
      } else {
        setError('Correo electrónico no encontrado');
      }
    } catch (error) {
      setError('Error al verificar el correo electrónico');
    }
  };

  return (
    <div className="login-container">
      <h2 className="text-2xl font-bold mb-4">Login</h2>
      <input
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        placeholder="Ingrese su correo electrónico"
        className="login-input mb-4 p-2 border border-gray-300 rounded"
      />
      <button onClick={handleLogin} className="login-button bg-blue-500 text-white px-4 py-2 rounded">
        Ingresar
      </button>
      {error && <div className="error text-red-500 mt-4">{error}</div>}
    </div>
  );
};

export default LoginPage;
