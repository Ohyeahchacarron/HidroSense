using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HidroSense.Data;
using HidroSense.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly HidroSenseContext _context;
    private readonly IConfiguration _config;

    public UsuariosController(HidroSenseContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registrar([FromBody] Usuario nuevoUsuario)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Correo == nuevoUsuario.Correo))
            return BadRequest("El correo ya está registrado.");

        if (await _context.Usuarios.AnyAsync(u => u.Telefono == nuevoUsuario.Telefono))
            return BadRequest("El teléfono ya está registrado.");

        nuevoUsuario.Nivel = "1";
        nuevoUsuario.EstablecerPassword(nuevoUsuario.PasswordHash);

        _context.Usuarios.Add(nuevoUsuario);
        await _context.SaveChangesAsync();

        return Ok("Usuario registrado correctamente.");
    }

    public class CredencialesLogin
    {
        public string Correo { get; set; }
        public string Password { get; set; }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] CredencialesLogin credenciales)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == credenciales.Correo);
        if (usuario == null || !usuario.VerificarPassword(credenciales.Password))
            return Unauthorized("Correo o contraseña incorrectos.");

        var token = GenerarJwt(usuario);

        return Ok(new
        {
            usuario.IdUsuario,
            usuario.Nombre,
            usuario.Correo,
            Token = token
        });
    }

    private string GenerarJwt(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Role, usuario.Nivel)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpireMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Authorize]
    [HttpPut("editar/{id}")]
    public async Task<IActionResult> EditarUsuario(int id, [FromBody] Usuario datos)
    {
        var usuarioObjetivo = await _context.Usuarios.FindAsync(id);
        if (usuarioObjetivo == null)
            return NotFound("Usuario no encontrado.");

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var userNivel = User.FindFirstValue(ClaimTypes.Role);


        //if (userNivel != "3" && userId != id)
        //    return Forbid("No tienes permiso para editar este usuario.");

        if (usuarioObjetivo.Correo != datos.Correo)
        {
            bool correoExiste = await _context.Usuarios.AnyAsync(u => u.Correo == datos.Correo && u.IdUsuario != id);
            if (correoExiste)
                return BadRequest("El correo ya está en uso.");
            usuarioObjetivo.Correo = datos.Correo;
        }

        usuarioObjetivo.Nombre = datos.Nombre;
        usuarioObjetivo.ApellidoPaterno = datos.ApellidoPaterno;
        usuarioObjetivo.ApellidoMaterno = datos.ApellidoMaterno;
        usuarioObjetivo.Edad = datos.Edad;
        usuarioObjetivo.Pais = datos.Pais;
        usuarioObjetivo.Telefono = datos.Telefono;
        usuarioObjetivo.Nivel = datos.Nivel;

        if (!string.IsNullOrEmpty(datos.PasswordHash))
            usuarioObjetivo.EstablecerPassword(datos.PasswordHash);

        await _context.SaveChangesAsync();
        return Ok("Usuario actualizado correctamente.");
    }

    [Authorize]
    [HttpDelete("eliminar/{id}")]
    public async Task<IActionResult> EliminarUsuario(int id)
    {
        var usuarioObjetivo = await _context.Usuarios.FindAsync(id);
        if (usuarioObjetivo == null)
            return NotFound("Usuario no encontrado.");

        _context.Usuarios.Remove(usuarioObjetivo);
        await _context.SaveChangesAsync();

        return Ok("Usuario eliminado.");
    }

}
