import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Dashboard from '@/pages/dashboard/Dashboard';
import Modal from '@/componentes/Modal/Modal'; // Importar el componente Modal
// import ProtectedRoute from '@/componentes/ProtectedRoute';

const API_URL = "http://localhost:5056/api/Ficha/qr";

interface QrCardData {
  qrImage: string;
  rut: string;
  nombres?: string;
  apellidoPaterno?: string;
  apellidoMaterno?: string;
  estado: string; // Asumiendo que este campo indica el estado del QR
  fechaCreacion: string; // Fecha de creación del QR
}

const QrList = () => {
  const [qrCards, setQrCards] = useState<QrCardData[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [isModalVisible, setIsModalVisible] = useState(false); // Estado para controlar la visibilidad del modal
  const [modalMessage, setModalMessage] = useState<string | null>(null); // Estado para el mensaje del modal

  useEffect(() => {
    fetchQrCards();
  }, []);

  const fetchQrCards = async () => {
    setLoading(true);
    setError(null);
    try {
      const qrResponse = await axios.get(`${API_URL}`);
      const qrData = qrResponse.data;

      const fichaPromises = qrData
        .filter((qr: QrCardData) => qr.estado === 'activo') // Filtrar solo los QR con estado activo
        .map(async (qr: QrCardData) => {
          try {
            const fichaResponse = await axios.get(`http://localhost:5056/api/Ficha/qr/ficha/${qr.rut}`);
            const fichaData = fichaResponse.data;
            if (fichaData && typeof fichaData === 'object') {
              return {
                ...qr,
                nombres: fichaData.nombres,
                apellidoPaterno: fichaData.apellidoPaterno,
                apellidoMaterno: fichaData.apellidoMaterno,
                fechaCreacion: qr.fechaCreacion,
              };
            } else {
              console.error(`Datos de ficha inválidos para RUT ${qr.rut}`);
              return qr;
            }
          } catch (error) {
            if (axios.isAxiosError(error) && error.response?.status === 404) {
              console.error(`Ficha no encontrada para RUT ${qr.rut}`);
              return qr; // Retornar el QR sin datos adicionales
            } else {
              console.error(`Error al obtener los datos de la ficha para RUT ${qr.rut}`, error);
              return qr; // Retornar el QR sin datos adicionales
            }
          }
        });

      const qrCardsWithFichaData = await Promise.all(fichaPromises);
      setQrCards(qrCardsWithFichaData);
    } catch (error) {
      console.error('Error al obtener los datos de los QR', error);
      setError('Error al obtener los datos de los QR');
    } finally {
      setLoading(false);
    }
  };

  const formatRut = (rut: string): string => {
    const rutBody = rut.slice(0, -1);
    const dv = rut.slice(-1);
    return `${rutBody}-${dv}`;
  };

  const isValidUrl = (url: string): boolean => {
    try {
      new URL(url);
      return true;
    } catch {
      return false;
    }
  };

  const handleDownload = (url: string, filename: string) => {
    if (!isValidUrl(url)) {
      console.error('URL no válida:', url);
      return;
    }
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    // Mostrar el modal con el mensaje de descarga
    setModalMessage('El QR ha sido descargado exitosamente.');
    setIsModalVisible(true);
  };

  const handleDeactivateQR = async (rut: string) => {
    setLoading(true);
    setError(null);
    setSuccessMessage(null);
    try {
      const response = await axios.post(`${API_URL}/deactivate/${rut}`);
      setSuccessMessage(response.data.message);
      fetchQrCards(); // Refrescar la lista de QR después de la operación

      // Mostrar el modal con el mensaje de desactivación
      setModalMessage('El QR ha sido desactivado exitosamente.');
      setIsModalVisible(true);
    } catch (error) {
      console.error('Error al desactivar el QR', error);
      setError('Error al desactivar el QR');
    } finally {
      setLoading(false);
    }
  };

  const closeModal = () => {
    setIsModalVisible(false);
    setModalMessage(null);
  };

  return (
    <Dashboard>
      {/* <ProtectedRoute> */}
        <div className="qrlist-container">
          <div className="search-bar-right">
            <input
              type="text"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              placeholder="Buscar por RUT o nombre"
              className="search-input-small"
            />
          </div>
          {loading && <div className="loading"><div className="spinner"></div><span>Cargando datos...</span></div>}
          {error && <div className="error-message">{error}</div>}
          {successMessage && <div className="success-message"></div>}
          <div className="qr-cards-container">
            {qrCards
              .filter(card => {
                const fullName = `${card.nombres} ${card.apellidoPaterno} ${card.apellidoMaterno}`.toLowerCase();
                return (
                  card.rut.toLowerCase().includes(searchTerm.toLowerCase()) ||
                  fullName.includes(searchTerm.toLowerCase())
                );
              })
              .map((card, index) => (
                <div key={index} className="qr-card">
                  <h3><strong>Funcionario:</strong> {card.nombres?.split(' ')[0]} {card.apellidoMaterno} {card.apellidoPaterno}</h3>
                  <div className="qr-image-container">
                    <img src={card.qrImage} alt="Código QR" className="qr-image" onError={(e) => (e.currentTarget.style.display = 'none')} />
                  </div>
                  <p><strong>RUT:</strong> {formatRut(card.rut)}</p>
                  <button onClick={() => handleDownload(card.qrImage, `QR_${card.rut}.png`)}>Descargar QR</button>
                  <button onClick={() => handleDeactivateQR(card.rut)}>Desactivar QR</button> {/* Botón para desactivar */}
                </div>
              ))}
          </div>
        </div>
      {/* </ProtectedRoute> */}

      <Modal isVisible={isModalVisible} onClose={closeModal}>
        <div className="modal-message">{modalMessage}</div>
      </Modal>
    </Dashboard>
  );
};

export default QrList;