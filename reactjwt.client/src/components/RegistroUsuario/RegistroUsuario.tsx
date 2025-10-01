//import React from "react";
import { useForm } from "./UseFormHandler";
import { useNavigate } from 'react-router-dom';

export default function Login() {
    const initialState = {
        userName: "",
        password_hash: "",
    };
    const navigate = useNavigate();

    function GoToPage(url: string) {
        try {
            navigate(url);
        } catch (error) {
            console.log("ha ocurrido un error con el redireccionado",error)
        }
        
    }

    async function LoginUserCallback() {
        try {
            const res = await fetch("http://localhost:5250/api/Auth/login", {
                method: "POST",
                headers: {
                    "Content-type": "application/json"
                },
                body: JSON.stringify(values),
                credentials: "include"
            });

            if (!res.ok) {
                throw new Error(`HTTP error! status: ${res.status}`);
            }

            //const result = await res.json();
            console.log("usuario agregado");
            const data = await res.json();
            const token = data.token;
            //console.log(token)
            
            GoToPage("/Bienvenido");



            //luego
            //crear una pagina que renderice usuarios validados
            //crear pagina para ingresar usuarios ya registrados con token


        } catch (error) {
            console.error("Error:", error);
        }
        
    }

    const { onChange, onSubmit, values } = useForm(
        LoginUserCallback,
        initialState
    );

    return (
        <>
            <h2>Registro de nuevo usuario</h2>
            <form onSubmit={onSubmit}>
                <div>
                    <input
                        name="userName"
                        id="userName"
                        type="text"
                        placeholder="UserName"
                        onChange={onChange}
                        required
                    />
                    <input
                        name="password_hash"
                        id="password_hash"
                        type="password"
                        placeholder="Password"
                        onChange={onChange}
                        required
                    />
                    <button type="submit">Login</button>
                </div>
            </form>
        </>
    );
}
