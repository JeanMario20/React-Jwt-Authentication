using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using ReactJwt.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ReactJwt.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] usersModels user)
        {
            string query = "SELECT password_hash, password_salt FROM users WHERE username = @username";
            MySqlConnection mConnection = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
            bool userFound = false;
            using (var cmd = new MySqlCommand(query, mConnection))
            {

                mConnection.Open();
                cmd.Parameters.AddWithValue("@username", user.userName);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string passwordHashDB = reader.GetString(0);
                        string passwordSaltDB = reader.GetString(1);
                        byte[] hashByte = Convert.FromBase64String(passwordHashDB);
                        byte[] saltByte = Convert.FromBase64String(passwordSaltDB);
                        var verifyPass = PasswordHaser.PasswordHasher.VerifyPasswordHash(user.password_hash, hashByte, saltByte);
                        userFound = verifyPass;
                    }
                }
                mConnection.Close();
            }

            if (userFound == true)
            {
                var token = GenerateJwtToken(user.userName);
                Response.Cookies.Append("token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    //SameSite = SameSiteMode.Strict,
                    SameSite = SameSiteMode.None, // <- como mi back y front estan en distintos puertos se pone asi
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });
            }

            return Ok(userFound);
            //admin1234 - admin1234
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] usersModels user)
        {

            try
            {
                string cmdText = "INSERT INTO users (username, password_hash, password_salt) VALUES (@username, @passwordHash, @passwordSalt)";
                MySqlConnection mConnection = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
                PasswordHaser.PasswordHasher.CreatePasswordHash(user.password_hash, out byte[] passwordHash, out byte[] passwordSalt);
                string hashString = Convert.ToBase64String(passwordHash);
                string saltString = Convert.ToBase64String(passwordSalt);
                user.password_hash = hashString;
                user.password_salt = saltString;

                using (var cmd = new MySqlCommand(cmdText, mConnection))
                {
                    mConnection.Open();
                    cmd.Parameters.AddWithValue("@username", user.userName);
                    cmd.Parameters.AddWithValue("@passwordHash", user.password_hash);
                    cmd.Parameters.AddWithValue("@passwordSalt", user.password_salt);
                    cmd.ExecuteNonQuery();
                    mConnection.Close();
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    Console.WriteLine("Error usuario ingresado duplicado en la Base de datos");
                    return Ok(new { duplicado = true, nuevoUsuario = false });
                }
                else
                {
                    Console.WriteLine($"MySQL Error: {ex.Message}");
                }
            }

            var token = GenerateJwtToken(user.userName);
            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                //SameSite = SameSiteMode.Strict,
                SameSite = SameSiteMode.None, // <- como mi back y front estan en distintos puertos se pone asi
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Ok(new { duplicado = false, nuevoUsario = true });

        }

        public class ValuesController : ControllerBase
        {
            [HttpGet]
            [Authorize]
            public IActionResult GetValues()
            {
                return Ok(new string[] { "value1", "value2" });
            }
        }



        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Jean2025!_SuperSecureKeyForJWT_Auth_Only"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
