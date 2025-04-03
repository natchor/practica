import React, { useState } from 'react';
import { login as apiLogin } from '../../Servicios/authService';
import TestConnection from '../../componentes/TestConnection';

import { useAuth } from '@/context/AuthContext';

const Login = () => {
  const { login } = useAuth();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isConnected, setIsConnected] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const handleConnectionChange = (status: boolean) => {
    setIsConnected(status);
  };

  const handleLogin = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!isConnected) {
      setError('No se puede conectar a la API');
      return;
    }
    if (!email || !password) {
      setError('Por favor, complete todos los campos');
      return;
    }
    try {
      const data = await apiLogin(email, password);
      login(data.token, data.userName);
    } catch (error) {
      if (error instanceof Error) {
        if (error.message.includes('401')) {
          setError('Correo o contraseña incorrecta');
        } else {
          setError('Correo o contraseña incorrecta');
        }
      } else {
        setError('Ocurrió un error desconocido');
      }
    }
  };

  return (
    <div className="container">
      <div className="login-box">
        <h2>Iniciar Sesión</h2>
        <TestConnection onConnectionChange={handleConnectionChange} />
        <form onSubmit={handleLogin}>
          <div className="mb-4">
            <label htmlFor="email"></label>
            <input
              type="email"
              id="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="Email"
              required
            />
          </div>
          <div className="mb-4 relative">
            <label htmlFor="password"></label>
            <input
              type={showPassword ? 'text' : 'password'}
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Contraseña"
              required
            />
            <button
              type="button"
              onClick={() => setShowPassword(!showPassword)}
              className="show-password-button"
            >
              {showPassword ? 'Ocultar' : 'Mostrar'}
            </button>
          </div>
          <button type="submit">Iniciar Sesión</button>
          {error && <p className="error">{error}</p>}
        </form>
      </div>
    </div>
  );
};

export default Login;