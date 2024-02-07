using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnet_JWT.Models;
using dotnet_JWT.DTO;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace dotnet_JWT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public static User user  = new User(); 
        private readonly IConfiguration _configuration; 

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration ;   
        }

        [HttpPost("register")]
        public ActionResult<User> Register (UserDTO request)
        {
            string passwordhash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            user.Username = request.Username;
            user.PasswordHash = passwordhash ; 


            return Ok(user);
        }


         [HttpPost("login")]
        public ActionResult<User> Login (UserDTO request)
        {
         
                if (user.Username != request.Username || !BCrypt.Net.BCrypt.Verify(request.Password , user.PasswordHash ))
                
                    return BadRequest("User or password not found");
                
            string token = CreateToken(user);
            return Ok(token);
        }

       private string CreateToken(User user)
{
    // Définition des revendications pour le token JWT
    List<Claim> claims = new List<Claim> 
    {
        new Claim(ClaimTypes.Name, user.Username)
    };

    // Récupération de la clé secrète depuis les configurations
    var tokenSecretKey = _configuration.GetSection("AppSettings:Token").Value!;
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecretKey));

    // Création des informations d'authentification
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    // Création du token JWT
    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );

    // Génération du token sous forme de chaîne
    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

    return jwt;
}

    }
}