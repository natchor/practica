import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/router';
import QRCode from 'react-qr-code';
import { getUserById } from '@/Servicios/apiService';
import { v4 as uuidv4 } from 'uuid';
import '../styles/qr.css';
import NavBar from '@/componentes/NavBar';

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

const QRPage = () => {
  const router = useRouter();
  const { userId, uniqueId } = router.query;
  const [user, setUser] = useState<UserData | null>(null);
  const [qrValue, setQrValue] = useState('');
  const [qrStyle, setQrStyle] = useState({});
  const [blockedUrls, setBlockedUrls] = useState<string[]>([]);

  useEffect(() => {
    if (userId) {
      const fetchUser = async () => {
        try {
          const userData = await getUserById(Number(userId));
          setUser(userData);
          generateQR(userData.id, uniqueId as string);
        } catch (error) {
          console.error('Error fetching user data:', error);
        }
      };

      fetchUser();
    }
  }, [userId, uniqueId]);

  const generateQR = (userId: number, uniqueId: string) => {
    const newQrValue = `${window.location.origin}/ficha/webinfo?id=${userId}&uniqueId=${uniqueId}`;
    setQrValue(newQrValue);
    setQrStyle({
      padding: '10px',
      borderRadius: '10px',
    });
  };

  const handleGenerateNewQR = () => {
    if (user) {
      const newUniqueId = uuidv4();
      generateQR(user.id, newUniqueId);
    }
  };

  const handleBlockQR = () => {
    setBlockedUrls([...blockedUrls, qrValue]);
    console.log('Bloquear QR', qrValue);
  };

  const handlePrintQR = () => {
    console.log('Imprimir QR');
  };

  const isBlocked = (url: string) => {
    return blockedUrls.includes(url);
  };

  return (
    <div>
      <NavBar />
      <div className="custom-container mx-auto p-4">
        <div className="container">
          <h2 className="text-2xl font-bold mb-4">QR Page</h2>
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
          <button onClick={handleGenerateNewQR} className="button">
            Generar Nuevo QR
          </button>
          <button onClick={handleBlockQR} className="button">
            Bloquear QR
          </button>
          <button onClick={handlePrintQR} className="button">
            Imprimir QR
          </button>
          {qrValue && !isBlocked(qrValue) && (
            <div className="qr-code" style={qrStyle}>
              <QRCode value={qrValue} />
            </div>
          )}
          {qrValue && isBlocked(qrValue) && (
            <div className="blocked-message">
              <p>El acceso a esta URL está bloqueado.</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default QRPage;