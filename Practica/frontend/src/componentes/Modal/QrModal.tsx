import React from 'react';


interface QrModalProps {
  isVisible: boolean;
  onClose: () => void;
}

const QrModal: React.FC<QrModalProps> = ({ isVisible, onClose }) => {
  if (!isVisible) return null;

  return (
    <div className="modal" onClick={onClose}>
      <div className="modal-content" onClick={e => e.stopPropagation()}>
        <h2>Código QR Generado</h2>
        {/* Aquí puedes agregar el contenido del modal */}
      </div>
    </div>
  );
};

export default QrModal;