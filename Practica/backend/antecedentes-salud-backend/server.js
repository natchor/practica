const express = require('express');
const app = express();
const port = 5056;

// Simulación de datos de fichas
const fichas = [
    {
        id: 1,
        rutCon: '12345678-9',
        nombres: 'Juan',
        apellidoMaterno: 'Perez',
        apellidoPaterno: 'Gomez',
        fechaNacimiento: '1990-01-01',
        alergias: 'Ninguna',
        medicamentos: 'Ninguno',
        enfermedades: 'Ninguna',
        mutualidad: 'Ninguna',
        grupoSanguineo: 'O+',
        factorRH: '+',
        obs: 'Ninguna',
        nombreCont: 'Maria',
        telefono: '123456789',
        direccionCont: 'Calle Falsa 123',
        qrImage: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAA...'
    },
    // Agrega más fichas según sea necesario
];

app.get('/api/ficha/:id', (req, res) => {
    const { id } = req.params;
    const ficha = fichas.find(f => f.id === parseInt(id));
    if (ficha) {
        res.json(ficha);
    } else {
        res.status(404).send('Ficha no encontrada');
    }
});

app.get('/api/ficha/qr', (req, res) => {
    const qrData = fichas.map(ficha => ({
        qrImage: ficha.qrImage,
        rut: ficha.rutCon,
        nombres: ficha.nombres,
        apellidoPaterno: ficha.apellidoPaterno
    }));
    res.json(qrData);
});

app.listen(port, () => {
    console.log(`Server running on port ${port}`);
});