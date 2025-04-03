import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/router';
import axios from 'axios';
import Modal from '@/componentes/Modal/Modal'; // Importar el componente Modal

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
  estado?: string; // Agregar el campo estado
}

interface User {
  id: number;
  rut: string;
  userName: string;
  nombre: string;
  correo: string;
  estado: number;
  rol: string;
  nombreFuncionario: string;
  telefonoFuncionario: string;
  direccionMinisterio: string;
}

const FichaPage = () => {
  const router = useRouter();
  const { hash } = router.query;

  const [ficha, setFicha] = useState<FichaData | null>(null);
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isModalVisible, setIsModalVisible] = useState(false); // Estado para controlar la visibilidad del modal
  const [modalMessage, setModalMessage] = useState<string | null>(null); // Estado para el mensaje del modal

  useEffect(() => {
    const fetchFicha = async () => {
      if (hash) {
        setLoading(true);
        setError(null);
        try {
          const response = await axios.get(`${API_URL}/api/Ficha/qr/ficha/${hash}`);
          if (response.data && typeof response.data === 'object') {
            if (response.data.estado === 'inactivo') {
              setModalMessage('El hash de esta persona está inactivo.');
              setIsModalVisible(true);
            } else {
              setFicha(response.data);
            }
          } else {
            setError('Datos de ficha inválidos');
          }
        } catch (error) {
          console.error('Error fetching data:', error);
          setError('Error al obtener los datos de la ficha');
        } finally {
          setLoading(false);
        }
      }
    };

    const fetchUsers = async () => {
      try {
        const response = await axios.get(`${API_URL}/api/Ficha/users`);
        if (response.data && Array.isArray(response.data)) {
          setUsers(response.data.filter((user: User) => user.estado === 1));
        } else {
          setError('Datos de usuarios inválidos');
        }
      } catch (error) {
        console.error('Error fetching users:', error);
        setError('Error al obtener los datos de los usuarios');
      }
    };

    fetchFicha();
    fetchUsers();
  }, [hash]);

  const closeModal = () => {
    setIsModalVisible(false);
    setModalMessage(null);
    router.push('/'); // Redirigir a la página principal o a otra página
  };

  const calculateAge = (birthDate: Date): number => {
    const today = new Date();
    const birthDateObj = new Date(birthDate);
    let age = today.getFullYear() - birthDateObj.getFullYear();
    const monthDifference = today.getMonth() - birthDateObj.getMonth();
    if (monthDifference < 0 || (monthDifference === 0 && today.getDate() < birthDateObj.getDate())) {
      age--;
    }
    return age;
  };

  if (loading) {
    return <div className="loading">Cargando...</div>;
  }

  if (error) {
    return <div className="error-message">{error}</div>;
  }

  if (!ficha) {
    return <div className="error-message">No se encontró la ficha</div>;
  }

  return (
    <div className="ficha-container">
      <h1 className="ficha-header">{ficha.nombres} {ficha.apellidoPaterno} {ficha.apellidoMaterno}</h1>
      <div className="ficha-content">
        <div className="ficha-field">
          <label><strong>RUT:</strong></label>
          <p>{ficha.rutCon}</p>
        </div>
        <div className="ficha-field">
          <label><strong>Fecha de Nacimiento:</strong></label>
          <p>{ficha.fechaNacimiento ? new Date(ficha.fechaNacimiento).toLocaleDateString() : 'No disponible'} ({calculateAge(ficha.fechaNacimiento)} años)</p>
        </div>
        <div className="ficha-field">
          <label><strong>Alergias:</strong></label>
          <p>{ficha.alergias}</p>
        </div>
        <div className="ficha-field">
          <label><strong>Medicamentos:</strong></label>
          <p>{ficha.medicamentos}</p>
        </div>
        <div className="ficha-field">
          <label><strong>Enfermedades:</strong></label>
          <p>{ficha.enfermedades}</p>
        </div>
        <div className="ficha-field">
          <label><strong>Mutualidad:</strong></label>
          <p>{ficha.mutualidad}</p>
        </div>
        <div className="ficha-field">
          <label><strong>Grupo Sanguíneo:</strong></label>
          <p>{ficha.grupoSanguineo}</p>
        </div>
        <div className="ficha-field">
          <label><strong>Factor RH:</strong></label>
          <p>{ficha.factorRH}</p>
        </div>
        <div className="ficha-field">
          <label><strong>Observaciones:</strong></label>
          <p>{ficha.obs}</p>
        </div>
      </div>
      
      <div className="users-content">
        {users.map(user => (
          <div key={user.id} className="user-field">
            <div className='ficha-field'>
              <label><strong>Nombre:</strong> </label>
              <p>{user.nombre}</p>
            </div>
            <div className='ficha-field'>
              <label><strong>Correo:</strong></label>
              <p>{user.correo}</p>
            </div>
            <div className='ficha-field'>
              <label><strong>Teléfono:</strong></label>
              <p>{user.telefonoFuncionario}</p>
            </div>
            <div className='ficha-field'>
              <label><strong>Dirección:</strong></label>
              <p>{user.direccionMinisterio}</p>
            </div>
          </div>
        ))}
      </div>

      <Modal isVisible={isModalVisible} onClose={closeModal}>
        <div className="modal-message">{modalMessage}</div>
      </Modal>
    </div>
  );
};

export default FichaPage;