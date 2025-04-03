import { AuthProvider } from '@/context/AuthContext';
import '@/pages/styles/QrList.css';
import '@/pages/styles/FichaPage.css';
import '@/pages/styles/login.css';
import '@/pages/styles/extractor.css';
import '@/pages/styles/GenerarQr.css';
import '@/pages/styles/global.css';
import '@/componentes/Modal/Modal.css';
import '@/pages/styles/Dashboard.css'
import { AppProps } from 'next/app';


console.log("API_BASE_URL:", process.env.API_BASE_URL); // Verifica el valor de API_BASE_URL

function MyApp({ Component, pageProps }: AppProps) {
  return (
    <AuthProvider>
      <Component {...pageProps} />
    </AuthProvider>
  );
}

export default MyApp;