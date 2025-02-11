import React from 'react';
import { useRouter } from 'next/router';
import '../pages/styles/Navbar.css'
import { useState } from 'react';


const NavBar = () => {
  const router = useRouter();
  const [showModal, setShowModal] = useState(false);

  const handleBack = () => {
    router.back();
  };

  const handleLogout = () => {
    localStorage.removeItem('user');
    router.push('/administracion/login');
  };

  const toggleModal = () => {
    setShowModal(!showModal);
  };

  return (
    <div className="navbar">
      <button onClick={handleBack} className="navbar-button">
        ← Regresar
      </button>
      <button onClick={toggleModal} className="navbar-button">
        ⎋ Cerrar sesión
      </button>

      {showModal && (
        <div className="modal">
          <div className="modal-content">
            <h2>¿Estás seguro de que deseas cerrar sesión?</h2>
            <button onClick={handleLogout} className="modal-button">
              Sí
            </button>
            <button onClick={toggleModal} className="modal-button">
              No
            </button>
          </div>
        </div>
      )}
    </div>
  );
};
  
  export default NavBar;