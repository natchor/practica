// pages/_app.tsx
import './styles/global.css';
import type { AppProps } from 'next/app';
import { useRouter } from 'next/router';
import { useEffect } from 'react';

function MyApp({ Component, pageProps }: AppProps) {
  const router = useRouter();

  useEffect(() => {
    const user = localStorage.getItem('user');
    if (!user && router.pathname !== '/administracion/login') {
      router.push('/administracion/login');
    }
  }, [router]);

  return <Component {...pageProps} />;
}

export default MyApp;