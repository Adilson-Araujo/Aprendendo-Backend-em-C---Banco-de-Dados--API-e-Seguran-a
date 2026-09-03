using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CadastroProdutos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Logincontroller : ControllerBase
    {
        private IConfiguration configuration;

        public Logincontroller(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            string role;
            // Validar os usuários
            if(login.Usuario == "admin" && login.Senha == "1234")
            {
                role = "admin";
            }
            else if(login.Usuario == "cliente" && login.Senha == "1234")
            {
                role = "cliente";
            }
            else
            {
                return Unauthorized();
            }

            // Criar o token JWT
            var jwtConfig = configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtConfig["Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("usuario", login.Usuario),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Validade do token
                Issuer = jwtConfig["Issuer"], // Emissor
                Audience = jwtConfig["Audience"], // Público - Quem pode utilizar
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new {Token = tokenString});
        }
    }
}