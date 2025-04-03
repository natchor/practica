import React, { useState } from 'react';
import { useRouter } from 'next/router';


const Sidebar: React.FC = () => {
  const router = useRouter();
  const [isOpen, setIsOpen] = useState(false);



  const toggleSidebar = () => {
    setIsOpen(!isOpen);
  };

  return (
    <nav className={`sidebar ${isOpen ? 'open' : ''}`}>
      <div className="logo-container">
        {/* <img src="../assets/logo.png" alt="Logo" className="logo" /> */}
      </div>
      <div className="nav-links">
        {/* <button onClick={() => router.push('')} className="nav-link">
          <i className="fas fa-users"></i>
          <span>Acceso</span>
        </button> */}
        <button onClick={() => router.push('/administracion/QrList')} className="nav-link">
          <i className="fas fa-qrcode"></i>
          <span>Lista de Codigos</span>
        </button>
        <button onClick={() => router.push('/administracion/extractor')} className="nav-link">
          <i className="fas fa-qrcode"></i>
          <span>Buscar Funcionario</span>
        </button>
      </div>
      <div className="user-info">
        <button onClick={() => router.push('/')} className="btn-logout">
          <i className="fas fa-sign-out-alt"></i>
          <span>Cerrar Sesión</span>
        </button>
      </div>
      <button className="toggle-button" onClick={toggleSidebar} title="Toggle Sidebar">
        <i className={`fas ${isOpen ? 'fa-times' : 'fa-bars'}`}></i>
      </button>
    </nav>
  );
};

export default Sidebar;