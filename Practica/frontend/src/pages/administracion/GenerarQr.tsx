import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import axios from 'axios';

const API_URL = "http://localhost:5056";

interface FormData {
  rut: string;
  nombre: string;
  apellido: string;
  telefono: string;
  contacto_emergencia: string;
  telefono_emergencia: string;
}

const GenerarQr: React.FC = () => {
  const { register, handleSubmit, formState: { errors } } = useForm<FormData>();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [qrGenerado, setQrGenerado] = useState<{ qrImage: string, rut: string } | null>(null);

  const onSubmit = async (data: FormData) => {
    setQrGenerado(null);
    setLoading(true);
    setError('');
    setQrGenerado(null);

    const qrData = JSON.stringify(data);

    try {
      const response = await axios.get(`${API_URL}/api/qr/generate`, { params: { url: qrData } });
      setQrGenerado({ qrImage: response.data.qrImage, rut: data.rut });
    } catch {
      setError('Error al generar el código QR');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="generar-qr-container">
      <form onSubmit={handleSubmit(onSubmit)}>
        <div>
          <label>RUT</label>
          <input {...register('rut', { required: true })} />
          {errors.rut && <span>Este campo es requerido</span>}
        </div>
        <div>
          <label>Nombre</label>
          <input {...register('nombre', { required: true })} />
          {errors.nombre && <span>Este campo es requerido</span>}
        </div>
        <div>
          <label>Apellido</label>
          <input {...register('apellido', { required: true })} />
          {errors.apellido && <span>Este campo es requerido</span>}
        </div>
        <div>
          <label>Teléfono</label>
          <input {...register('telefono', { required: true })} />
          {errors.telefono && <span>Este campo es requerido</span>}
        </div>
        <div>
          <label>Contacto de Emergencia</label>
          <input {...register('contacto_emergencia', { required: true })} />
          {errors.contacto_emergencia && <span>Este campo es requerido</span>}
        </div>
        <div>
          <label>Teléfono de Emergencia</label>
          <input {...register('telefono_emergencia', { required: true })} />
          {errors.telefono_emergencia && <span>Este campo es requerido</span>}
        </div>
        <button type="submit" disabled={loading}>Generar QR</button>
      </form>
      {error && <div className="error-message">{error}</div>}
      {qrGenerado && (
        <div className="qr-result">
          
          <img src={qrGenerado.qrImage} alt="Código QR" />
          <p><strong>RUT:</strong> {qrGenerado.rut}</p>
        </div>
      )}
    </div>
  );
};

export default GenerarQr;