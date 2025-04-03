import React from 'react';
import LoginPage from './administracion/login';
import dotenv from 'dotenv';

dotenv.config();

const Page = () => {
  return (
    <div>
      <LoginPage/>
    </div>
  );
};

export default Page;