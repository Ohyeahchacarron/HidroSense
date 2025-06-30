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

        nuevoUsuario.EstablecerPassword(nuevoUsuario.PasswordHash);
        _context.Usuarios.Add(nuevoUsuario);
        await _context.SaveChangesAsync();
        return Ok("Usuario registrado correctamente.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Usuario usuarioLogin)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == usuarioLogin.Correo);

        if (usuario == null || !usuario.VerificarPassword(usuarioLogin.PasswordHash))
        {
            return Unauthorized("Correo o contraseña incorrectos.");
        }

        var tokenPlano = Guid.NewGuid().ToString();

        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(tokenPlano));
        usuario.Token = Convert.ToBase64String(hash);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            usuario.IdUsuario,
            usuario.Nombre,
            usuario.Correo,
            Token = tokenPlano 
        });
    }

    [HttpPut("editar/{id}")]
    public async Task<IActionResult> EditarUsuario(int id, [FromBody] Usuario datosEditados, [FromQuery] string token)
    {
        var tokenEncriptado = Usuario.EncriptarToken(token);
        var usuarioAutenticado = await _context.Usuarios.FirstOrDefaultAsync(u => u.Token == tokenEncriptado);

        if (usuarioAutenticado == null)
            return Unauthorized("Token inválido.");

        var usuarioObjetivo = await _context.Usuarios.FindAsync(id);
        if (usuarioObjetivo == null)
            return NotFound("Usuario no encontrado.");

        if (usuarioAutenticado.Nivel != "3" && usuarioAutenticado.IdUsuario != id)
            return Forbid("No tienes permiso para editar este usuario.");

        if (usuarioObjetivo.Correo != datosEditados.Correo)
        {
            bool correoExiste = await _context.Usuarios.AnyAsync(u => u.Correo == datosEditados.Correo && u.IdUsuario != id);
            if (correoExiste)
                return BadRequest("El correo ya está en uso por otro usuario.");

            usuarioObjetivo.Correo = datosEditados.Correo;
        }

        usuarioObjetivo.Nombre = datosEditados.Nombre;
        usuarioObjetivo.Telefono = datosEditados.Telefono;
        usuarioObjetivo.Nivel = datosEditados.Nivel;

        await _context.SaveChangesAsync();

        return Ok("Usuario actualizado correctamente.");
    }

    [HttpDelete("eliminar/{id}")]
    public async Task<IActionResult> EliminarUsuario(int id, [FromQuery] string token)
    {
        var tokenEncriptado = Usuario.EncriptarToken(token);
        var usuarioAutenticado = await _context.Usuarios.FirstOrDefaultAsync(u => u.Token == tokenEncriptado);

        if (usuarioAutenticado == null)
            return Unauthorized("Token inválido.");

        var usuarioObjetivo = await _context.Usuarios.FindAsync(id);
        if (usuarioObjetivo == null)
            return NotFound("Usuario no encontrado.");

        if (usuarioAutenticado.Nivel != "3" && usuarioAutenticado.IdUsuario != id)
            return Forbid("No tienes permiso para eliminar este usuario.");

        _context.Usuarios.Remove(usuarioObjetivo);
        await _context.SaveChangesAsync();

        return Ok("Usuario eliminado correctamente.");
    }


}
