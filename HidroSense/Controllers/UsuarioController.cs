using System.Security.Cryptography;
using System.Text;
using HidroSense.Data;
using HidroSense.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly HidroSenseContext _context;

    public UsuariosController(HidroSenseContext context)
    {
        _context = context;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registrar([FromBody] Usuario nuevoUsuario)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Correo == nuevoUsuario.Correo))
        {
            return BadRequest("El correo ya está registrado.");
        }

        nuevoUsuario.Nivel = "1";
        nuevoUsuario.EstablecerPassword(nuevoUsuario.PasswordHash);

        _context.Usuarios.Add(nuevoUsuario);
        await _context.SaveChangesAsync();

        return Ok(new { 
            status = 200,
            message = "Usuario registrado correctamente."
        });
    }




    public class CredencialesLogin
    {
        public string Correo { get; set; }
        public string Password { get; set; }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] CredencialesLogin credenciales)
    {

        try
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == credenciales.Correo);

            if (usuario == null || !usuario.VerificarPassword(credenciales.Password))
            {
                return Unauthorized("Correo o contraseña incorrectos.");
            }

            var tokenPlano = Guid.NewGuid().ToString();
            usuario.Token = Usuario.EncriptarToken(tokenPlano);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                usuario.IdUsuario,
                usuario.Nombre,
                usuario.Correo,
                Token = tokenPlano,
                status = 200
            });
        }
        catch (Exception e) {
            return StatusCode(500, 
                new { 
                idUsuario =  0,
                nombre = "",
                correo = "null",
                token = "null",
                status = 500});
                }
     
    }

    [HttpPut("editar/{id}")]
    public async Task<IActionResult> EditarUsuario(int id, [FromBody] Usuario datos)
    {
        if (string.IsNullOrEmpty(datos.Token))
            return BadRequest("Se requiere el token.");

        var tokenEncriptado = Usuario.EncriptarToken(datos.Token);
        var usuarioAutenticado = await _context.Usuarios.FirstOrDefaultAsync(u => u.Token == tokenEncriptado);

        if (usuarioAutenticado == null)
            return Unauthorized("Token inválido.");

        var usuarioObjetivo = await _context.Usuarios.FindAsync(id);
        if (usuarioObjetivo == null)
            return NotFound("Usuario no encontrado.");

        if (usuarioAutenticado.Nivel != "3" && usuarioAutenticado.IdUsuario != id)
            return Forbid("No tienes permiso para editar este usuario.");

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


    public class EliminacionRequest
    {
        public string Token { get; set; }
    }

    [HttpDelete("eliminar/{id}")]
    public async Task<IActionResult> EliminarUsuario(int id, [FromBody] EliminacionRequest datos)
    {
        var tokenEncriptado = Usuario.EncriptarToken(datos.Token);
        var usuarioAutenticado = await _context.Usuarios.FirstOrDefaultAsync(u => u.Token == tokenEncriptado);

        if (usuarioAutenticado == null)
            return Unauthorized("Token inválido.");

        var usuarioObjetivo = await _context.Usuarios.FindAsync(id);
        if (usuarioObjetivo == null)
            return NotFound("Usuario no encontrado.");

        _context.Usuarios.Remove(usuarioObjetivo);
        await _context.SaveChangesAsync();

        return Ok("Usuario eliminado.");
    }
}
