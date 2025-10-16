//import { useEffect, useState } from 'react';
import Register from './components/LoginUsers/LoginUser'
import Login from './components/RegisterUser/RegistroUser'
import BienvenidaUsuario from './components/BienvenidaUsuario/BienvenidaUsuario'
//import Register from './components/RegistroUsuario/RegistroUsuario'
//import Login from './components/LoginUsuario/LoginUsuario'
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import React from 'react';

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Register />} />
                <Route path="/Bienvenido" element={<BienvenidaUsuario />} />
                <Route path="/Login" element = {<Login/>}/>
            </Routes>
        </BrowserRouter>
    )
}