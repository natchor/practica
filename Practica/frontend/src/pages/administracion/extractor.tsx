import React, { useState, useEffect } from 'react';
import { getAllFichas } from '@/Servicios/fichaService';
import axios from 'axios';
import Dashboard from '@/pages/dashboard/Dashboard';
import Modal from '@/componentes/Modal/Modal';
import QrModal from '@/componentes/Modal/QrModal';

// import ProtectedRoute from '@/componentes/ProtectedRoute';

const API_URL = "http://localhost:5056";

interface FichaData {
  id: number;
  rutCon: string;
  nombres?: string;
  apellidoMaterno?: string;
  apellidoPaterno?: string;
  fechaNacimiento: Date;
  alergias?: string;
  medicamentos?: string;
  enfermedades?: string;
  mutualidad?: string;
  grupoSanguineo?: string;
  factorRH?: string;
  obs?: string;
  nombreCont?: string;
  telefono?: string;
  direccionCont?: string;
  nombreCont2?: string;
  telefono2?: string;
  direccionCont2?: string;
  nombreCont3?: string;
  telefono3?: string;
  direccionCont3?: string;
}

interface QrCardData {
  qrImage: string;
  rut: string;
  nombres: string;
  apellidoPaterno: string;
}

const Extractor = () => {
  const [fichas, setFichas] = useState<FichaData[]>([]);
  const [filteredFichas, setFilteredFichas] = useState<FichaData[]>([]);
  const [selectedFicha, setSelectedFicha] = useState<FichaData | null>(null);
  const [searchRut, setSearchRut] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [isQrModalVisible, setIsQrModalVisible] = useState(false);
  const [qrCards, setQrCards] = useState<QrCardData[]>([]);
  const [qrExists, setQrExists] = useState(false);
  const [qrMessage, setQrMessage] = useState<string | null>(null);
  const [currentView, setCurrentView] = useState(1); // Estado para controlar la vista actual
  const [qrErrorMessage, setQrErrorMessage] = useState<string | null>(null); // Estado para el mensaje de error del QR
  const [qrExistsMessage, setQrExistsMessage] = useState<string | null>(null); // Estado para el mensaje de QR existente

  useEffect(() => {
    const fetchFichas = async () => {
      setLoading(true);
      setError(null);
      try {
        const fichasData = await getAllFichas();
        setFichas(fichasData.map(ficha => ({
          ...ficha,
          fechaNacimiento: new Date(ficha.fechaNacimiento),
        })));
        setFilteredFichas(fichasData.map(ficha => ({
          ...ficha,
          fechaNacimiento: new Date(ficha.fechaNacimiento),
        })));
      } catch {
        console.error('Error al obtener los datos de las fichas');
        setError('Error al obtener los datos de las fichas');
      } finally {
        setLoading(false);
      }
    };

    fetchFichas();
  }, []);

  const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
    const term = e.target.value.toLowerCase().replace(/[^a-zA-Z0-9]/g, '');
    setSearchRut(term);
    if (term) {
      const filtered = fichas.filter(ficha => {
        const fullName = `${ficha.nombres} ${ficha.apellidoPaterno} ${ficha.apellidoMaterno}`.toLowerCase();
        return (
          ficha.rutCon.startsWith(term) ||
          fullName.includes(term)
        );
      }).sort((a, b) => a.rutCon.localeCompare(b.rutCon));
      setFilteredFichas(filtered);
    } else {
      setFilteredFichas(fichas);
    }
  };

  const handleSelectFicha = (ficha: FichaData) => {
    setSelectedFicha(ficha);
    setSearchRut('');
    setFilteredFichas([]);
    setIsModalVisible(true);
  };

  const closeModal = () => {
    setIsModalVisible(false);
    setSelectedFicha(null);
  };

  const closeQrModal = () => {
    setIsQrModalVisible(false);
    setQrExists(false);
    setQrMessage(null);
    setQrErrorMessage(null); // Limpiar el mensaje de error del QR
    setQrExistsMessage(null); // Limpiar el mensaje de QR existente
  };

  const generateQr = async () => {
    if (selectedFicha) {
      try {
        // Verificar si el funcionario ya tiene un QR generado
        const response = await axios.get(`${API_URL}/api/ficha/qr/${selectedFicha.rutCon}`);
        if (response.data.qrData) {
          if (response.data.qrData.estado === 'activo') {
            setQrExists(true);
            setQrExistsMessage('El funcionario ya tiene un QR activo.');
            setIsQrModalVisible(true);
            return;
          } else {
            // Si el QR está desactivado, generar un nuevo QR
            await createNewQr();
            setQrMessage('Se ha creado un nuevo QR.');
            setIsQrModalVisible(true);
            return;
          }
        }
      } catch (error) {
        // Si no se encuentra el QR, continuar con la generación
        if (axios.isAxiosError(error) && error.response && error.response.status !== 404) {
          setError('Error al verificar el código QR');
          return;
        }
      }

      try {
        // Solicitar al backend que genere el QR
        await createNewQr();
        setQrMessage('Se ha creado un nuevo QR.');
        setIsQrModalVisible(true);
      } catch (error) {
        if (axios.isAxiosError(error) && error.response) {
          setQrErrorMessage(`Error al generar el código QR: ${error.response.data.message || error.message}`);
        } else {
          setQrErrorMessage('Error al generar el código QR');
        }
        setIsQrModalVisible(true);
      }
    }
  };

  const createNewQr = async () => {
    try {
      const response = await axios.post(`${API_URL}/api/ficha/qr`, {
        rut: selectedFicha!.rutCon,
        nombres: selectedFicha!.nombres,
        apellidoMaterno: selectedFicha!.apellidoMaterno,
        apellidoPaterno: selectedFicha!.apellidoPaterno,
        estado: 'activo',
        fechaCreacion: new Date().toISOString(),
        fechaEliminacion: new Date(0).toISOString(), // Fecha mínima
      });

      const newQrCard: QrCardData = {
        qrImage: response.data.qrCodeImage,
        rut: selectedFicha!.rutCon,
        nombres: selectedFicha!.nombres || '',
        apellidoPaterno: selectedFicha!.apellidoPaterno || ''
      };

      setQrCards(prevQrCards => {
        const updatedQrCards = [newQrCard, ...prevQrCards];
        return updatedQrCards.slice(0, 4); // Limitar a 4 tarjetas
      });
    } catch (error) {
      console.error('Error al generar el código QR', error);
      if (axios.isAxiosError(error) && error.response) {
        setQrErrorMessage(`Error al generar el código QR: ${error.response.data.message || error.message}`);
      } else {
        setQrErrorMessage('Error al generar el código QR');
      }
    }
  };

  const formatRut = (rut: string): string => {
    const rutBody = rut.slice(0, -1);
    const dv = rut.slice(-1);
    return `${rutBody}-${dv}`;
  };

  const handleViewChange = () => {
    setCurrentView(currentView === 1 ? 2 : 1);
  };

  return (
    // <ProtectedRoute>
      <Dashboard>
        <div className="funcionario-container">
          <div className="card">
            <div className="search-bar">
              <input
                type="text"
                value={searchRut}
                onChange={handleSearch}
                placeholder="Ingrese el RUT, nombre o apellido"
                className="search-input"
              />
            </div>
            {searchRut && (
              <div className="search-results">
                {filteredFichas.length > 0 ? (
                  <ul className="results-list">
                    {filteredFichas.map(ficha => (
                      <li key={ficha.id} onClick={() => handleSelectFicha(ficha)} className="result-item">
                        {formatRut(ficha.rutCon)} - {ficha.nombres} {ficha.apellidoPaterno} {ficha.apellidoMaterno}
                      </li>
                    ))}
                  </ul>
                ) : (
                  <div className="error-message">No se encontraron datos</div>
                )}
              </div>
            )}

            {loading && <div className="loading"><div className="spinner"></div><span>Cargando datos...</span></div>}
            {error && <div className="error-message">{error}</div>}

            <Modal isVisible={isModalVisible} onClose={closeModal}>
              {selectedFicha && (
                <div className="funcionario-data">
                  <div className="header">
                    <h2>{selectedFicha.nombres} {selectedFicha.apellidoPaterno}</h2>
                  </div>
                  <div className="header">
                  <div className="rut">{formatRut(selectedFicha.rutCon)}</div></div>
                  <div className="content-grid">
                    {currentView === 1 ? (
                      <div className="column">
                        <div className="info-section">
                          <div className="info-group">
                            <label>Fecha de Nacimiento</label>
                            <p>{selectedFicha.fechaNacimiento.toLocaleDateString()}</p>
                          </div>
                          <div className="info-group">
                            <label>Alergias</label>
                            <p>{selectedFicha.alergias || 'Ninguna'}</p>
                          </div>
                          <div className="info-group">
                            <label>Medicamentos</label>
                            <p>{selectedFicha.medicamentos || 'Ninguno'}</p>
                          </div>
                          <div className="info-group">
                            <label>Enfermedades</label>
                            <p>{selectedFicha.enfermedades || 'Ninguna'}</p>
                          </div>
                          <div className="info-group">
                            <label>Observaciones</label>
                            <p>{selectedFicha.obs || 'Sin observaciones'}</p>
                          </div>
                          <div className="info-group">
                            <label>Mutualidad</label>
                            <p>{selectedFicha.mutualidad}</p>
                          </div>
                          <div className="info-group">
                            <label>Grupo Sanguíneo</label>
                            <p>{selectedFicha.grupoSanguineo}</p>
                          </div>
                          <div className="info-group">
                            <label>Factor RH</label>
                            <p>{selectedFicha.factorRH}</p>
                          </div>
                        </div>
                      </div>
                    ) : (
                      <div className="column">
                        <div className="info-section">
                          <div className="info-group">
                            <label>Contacto 1</label>
                            <p>{selectedFicha.nombreCont}</p>
                            <p>{selectedFicha.telefono}</p>
                          </div>
                          <div className="info-group">
                            <label>Contacto 2</label>
                            <p>{selectedFicha.nombreCont2}</p>
                            <p>{selectedFicha.telefono2}</p>
                          </div>
                          <div className="info-group">
                            <label>Contacto 3</label>
                            <p>{selectedFicha.nombreCont3}</p>
                            <p>{selectedFicha.telefono3}</p>
                          </div>
                        </div>
                      </div>
                    )}
                  </div>
                  <button onClick={handleViewChange} className="btn">
                    {currentView === 1 ? 'Ver Contactos' : 'Ver Información Personal'}
                  </button>
                  <button onClick={generateQr} className="btn">Generar QR</button>
                </div>
              )}
            </Modal>

            <Modal isVisible={qrExists} onClose={closeQrModal}>
              <div className="error-message">{qrMessage}</div>
            </Modal>

            <Modal isVisible={isQrModalVisible} onClose={closeQrModal}>
              {qrExistsMessage ? (
                <div className="error-message">{qrExistsMessage}</div>
              ) : qrErrorMessage ? (
                <div className="error-message">{qrErrorMessage}</div>
              ) : (
                <div className="success-message">{qrMessage}</div>
              )}
            </Modal>

            <QrModal
              isVisible={isQrModalVisible}
              onClose={closeQrModal}
            />

            <div className="qr-cards-container">
              {qrCards.map((card, index) => (
                <div key={index} className="qr-card">
                  <img src={card.qrImage} alt="Código QR" />
                  <p><strong>RUT:</strong> {formatRut(card.rut)}</p>
                  <p><strong>Nombre:</strong> {card.nombres} {card.apellidoPaterno}</p>
                </div>
              ))}
            </div>
          </div>
        </div>
      </Dashboard>
    // </ProtectedRoute>
  );
};

export default Extractor;