//import { useEffect, useState } from 'react';
import Login from './components/RegistroUsuario/RegistroUsuario'
import BienvenidaUsuario from './components/BienvenidaUsuario/BienvenidaUsuario'
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import React from 'react';

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Login />} />
                <Route path="/Bienvenido" element={<BienvenidaUsuario />} />
            </Routes>
        </BrowserRouter>
    )
}