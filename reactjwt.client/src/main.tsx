import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import React from 'react'
import './index.css'
//import App from './App.tsx'
import Login from './components/RegistroUsuario/RegistroUsuario'
import BienvenidaUsuario from './components/BienvenidaUsuario/BienvenidaUsuario'
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import App from './App.tsx'

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <App/>
    </StrictMode>,
)
