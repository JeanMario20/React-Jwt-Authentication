import { useForm } from "../LoginUsers/UseFormHandler";
import { useNavigate } from 'react-router-dom';
function LoginUsuario() {

    const initialState = {
        userName: "",
        password_hash: "",
        password_salt: ""
    }

    const navigate = useNavigate();

    function GoToPage(url: string) {
        try {
            navigate(url);
        } catch (error) {
            console.log("ha ocurrido un error con el redireccionado", error)
        }

    }

    async function LoginUserCallback() {
        try {
            const res = await fetch("http://localhost:5250/api/Auth/Register", {
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


            const data = await res.json();
            if (data.nuevoUsuario == true) {
                GoToPage("/Bienvenido");
            }
            if (data.duplicado == true) {
                alert("usuario ya se encuentra en la base de datos")
            }



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
          <h2>ingresa nuevo usuario.</h2>
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
                  <button type="submit">Registrar</button>
              </div>
          </form>
      </>
  );
}

export default LoginUsuario;