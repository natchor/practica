import React, { useState } from 'react';
import { useRouter } from 'next/router';
import { getUsers } from '@/Servicios/apiService';
import '../styles/login.css';

const LoginPage = () => {
  const router = useRouter();
  const [email, setEmail] = useState('');
  const [error, setError] = useState<string | null>(null);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
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
    <div className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8 bg-gray-50">
      <div className="sm:mx-auto sm:w-full sm:max-w-md">
        <h2 className="mt-6 text-center text-3xl font-bold tracking-tight text-gray-900">
          Iniciar sesión en tu cuenta
        </h2>
      </div>

      <div className="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
        <div className="bg-white py-8 px-6 shadow rounded-lg sm:px-10">
          <form onSubmit={handleLogin} className="space-y-6">
            <div>
              <label htmlFor="email" className="block text-sm font-medium text-gray-700">
                Correo electrónico
              </label>
              <div className="mt-1">
                <input
                  id="email"
                  name="email"
                  type="email"
                  required
                  autoComplete="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className="block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                />
              </div>
            </div>

            <div>
              <button
                type="submit"
                className="flex w-full justify-center rounded-md border border-transparent bg-indigo-600 py-2 px-4 text-sm font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
              >
                Iniciar sesión
              </button>
            </div>
          </form>

          {error && <div className="mt-4 text-center text-red-500">{error}</div>}
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
