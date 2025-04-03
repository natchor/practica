import React from 'react';
import dynamic from 'next/dynamic';
import { Routes, Route } from 'react-router-dom';
import Extractor from '@/pages/administracion/extractor';
import QrList from '@/pages/administracion/QrList';

// Cargar dinámicamente BrowserRouter solo en el cliente
const BrowserRouter = dynamic(() => import('react-router-dom').then(mod => mod.BrowserRouter), { ssr: false });

const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/administracion/extractor" element={<Extractor />} />
        <Route path="/administracion/qr-list" element={<QrList />} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;