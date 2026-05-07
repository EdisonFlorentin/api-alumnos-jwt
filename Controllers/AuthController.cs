namespace apiAlumnos.Controllers
{
    using apiAlumnos.Data;
    using apiAlumnos.DTOs;
    using apiAlumnos.Models;
    using Microsoft.AspNetCore.Mvc;
    using BCrypt.Net;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDTO dto)
        {
            var userExists = _context.Usuarios.Any(u => u.Email == dto.Email);

            if (userExists)
                return BadRequest("El usuario ya existe");

            var usuario = new Usuario
            {
                Email = dto.Email,
                PasswordHash = BCrypt.HashPassword(dto.Password),
                Rol = "User" // por defecto
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok("Usuario registrado");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == dto.Email);

            if (usuario == null)
                return Unauthorized("Credenciales inválidas");

            bool passwordValida = BCrypt.Verify(dto.Password, usuario.PasswordHash);

            if (!passwordValida)
                return Unauthorized("Credenciales inválidas");

            //  CLAVE CORRECTA
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            //  TOKEN SIN issuer/audience (para evitar error)
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString });
        }
    }
}
