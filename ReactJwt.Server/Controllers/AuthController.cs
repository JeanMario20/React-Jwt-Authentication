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

        [HttpPost("login")]
        public IActionResult Login([FromBody] usersModels user)
        {
            string query = "SELECT username, password_hash FROM users WHERE username = @username";
            MySqlConnection mConnection = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
            using (var cmd = new MySqlCommand(query, mConnection))
            {
                Console.WriteLine(user.userName);
                Console.WriteLine("usuario encontrado");
            }

            return Ok(new { message = "Usuario encontrado" });

            /*if (user.userName == "admin" && user.password_hash == "password")
            {
                var token = GenerateJwtToken(user.userName);
                return Ok(new { token });
            }
            return Unauthorized();*/
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] usersModels user)
        {

            var token = GenerateJwtToken(user.userName);


            string cmdText = "INSERT INTO users (username, password_hash) VALUES (@username, @password)";
            MySqlConnection mConnection = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));

            using (var cmd = new MySqlCommand(cmdText, mConnection))
            {
                mConnection.Open();
                cmd.Parameters.AddWithValue("@username", user.userName);
                cmd.Parameters.AddWithValue("@password", user.password_hash);
                cmd.ExecuteNonQuery();
            }

            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                //SameSite = SameSiteMode.Strict,
                SameSite = SameSiteMode.None, // <- como mi back y front estan en distintos puertos se pone asi
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Ok(new { message = "Usuario registrado" });

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
