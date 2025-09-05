//import React from "react";
import { useForm } from "./UseFormHandler";

export default function Login() {
    const initialState = {
        userName: "",
        password_hash: "",
    };

    async function loginUserCallback() {
        try {
            const res = await fetch("https://localhost:7101/api/Auth/Register", {
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
            //setResponse(JSON.stringify(result)); se pone un useState;
            //hacer a continuacion ingresar nuevos usuarios
            //crear jwtTokenCode
            //almacenar base de datos

            //luego
            //crear una pagina que renderice usuarios validados
            //crear pagina para ingresar usuarios ya registrados con token


        } catch (error) {
            console.error("Error:", error);
        }
        
    }

    const { onChange, onSubmit, values } = useForm(
        loginUserCallback,
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
                        name="password"
                        id="password"
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
