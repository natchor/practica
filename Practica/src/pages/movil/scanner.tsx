import React, { useState } from 'react';
import { useRouter } from 'next/router';
import { QrReader } from 'react-qr-reader';
import '../styles/scan.css';

const ScannerPage = () => {
  const router = useRouter();
  const [error, setError] = useState<string | null>(null);
  const [cameraActive, setCameraActive] = useState<boolean>(false);
  const [facingMode, setFacingMode] = useState<'user' | 'environment'>('environment');

  const handleScan = (data: string | null) => {
    if (data) {
      const url = new URL(data);
      const userId = url.searchParams.get('id');
      if (userId) {
        router.push(`/Page mobile/user?id=${userId}`);
      } else {
        setError('Código QR inválido');
      }
    }
  };

  const handleError = (err: any) => {
    setError('Error al escanear el código QR');
    console.error(err);
  };

  const toggleFacingMode = () => {
    setFacingMode((prevMode) => (prevMode === 'user' ? 'environment' : 'user'));
  };

  const activateCamera = () => {
    setCameraActive(true);
  };

  return (
    <div className="scanner-container">
      <h2 className="text-2xl font-bold mb-4">Escanear QR</h2>
      {!cameraActive && (
        <button onClick={activateCamera} className="activate-button">
          Activar Cámara
        </button>
      )}
      {cameraActive && (
        <>
          <button onClick={toggleFacingMode} className="toggle-button">
            Cambiar Cámara
          </button>
          <div className="camera-view">
            <QrReader
              constraints={{ facingMode }}
              onResult={(result, error) => {
                if (result) {
                  handleScan(result.getText());
                }
                if (error) {
                  handleError(error);
                }
              }}
            />
            <div className="scan-area"></div>
          </div>
          <div className="camera-active">Cámara Activa</div>
        </>
      )}
      {error && <div className="error text-red-500 mt-4">{error}</div>}
    </div>
  );
};

export default ScannerPage;