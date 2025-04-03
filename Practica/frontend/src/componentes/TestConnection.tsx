import { useEffect } from 'react';

const TestConnection = ({ onConnectionChange }: { onConnectionChange: (isConnected: boolean) => void }) => {
  useEffect(() => {
    const checkConnection = async () => {
      try {
        const response = await fetch('');
        if (!response.ok) {
          throw new Error('Error en la respuesta de la API');
        }
        onConnectionChange(true);
      } catch (error) {
        console.error('Error:', error);
        onConnectionChange(false);
      }
    };

    checkConnection();
  }, [onConnectionChange]);

  return null; // No necesitas renderizar nada
};

export default TestConnection;