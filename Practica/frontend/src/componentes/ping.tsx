import React, { useEffect } from 'react';
import axios from 'axios';

const PingTest = () => {
  useEffect(() => {
    axios.get(process.env.API_URL + '/api/Ficha/ping')
      .then(response => {
        console.log('Ping response:', response.data);
      })
      .catch(error => {
        console.error('Error pinging API:', error);
      });
  }, []);

  return (
    <div>
      <h1>Ping Test</h1>
      <p>Check the console for the ping response.</p>
    </div>
  );
};

export default PingTest;