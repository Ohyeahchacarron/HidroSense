using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HidroSense.Data;
using HidroSense.Models;
using HidroSense.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[Authorize]
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



    [HttpGet("getUsuarios")]
    public async Task<ActionResult<List<Usuario>>> getUsuarios()
    {

        var usuarios = await _context.Usuarios.ToListAsync();

        return Ok(usuarios);
    }



    [AllowAnonymous]
    [HttpPost("registro")]
    public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDTO dto)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo))
            return BadRequest(new { success = false, message = "El correo ya está registrado.", data = (object)null });

        if (await _context.Usuarios.AnyAsync(u => u.Telefono == dto.Telefono))
            return BadRequest(new { success = false, message = "El teléfono ya está registrado.", data = (object)null });

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            ApellidoPaterno = dto.ApellidoPaterno,
            ApellidoMaterno = dto.ApellidoMaterno,
            Edad = dto.Edad,
            Pais = dto.Pais,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            Nivel = dto.Nivel
        };

        usuario.EstablecerPassword(dto.Contrasenia);

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Registro exitoso, nuevo usuario",
            data = new
            {
                usuario.IdUsuario,
                usuario.Nombre,
                usuario.ApellidoPaterno,
                usuario.ApellidoMaterno,
                usuario.Edad,
                usuario.Pais,
                usuario.Correo,
                Contrasenia = dto.Contrasenia,
                usuario.Telefono
            }
        });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == dto.Correo);

        if (usuario == null || !usuario.VerificarPassword(dto.Contrasenia))
            return Unauthorized(new { success = false, message = "Correo o contraseña incorrectos.", data = (object)null });

        var token = GenerarJwt(usuario);

        return Ok(new
        {
            success = true,
            message = "Credenciales correctas",
            data = new
            {
                token,
                usuario.IdUsuario,
                usuario.Nombre,
                usuario.ApellidoPaterno,
                usuario.ApellidoMaterno,
                usuario.Edad,
                usuario.Pais,
                usuario.Correo,
                Contrasenia = dto.Contrasenia,
                usuario.Telefono,
                usuario.Nivel
            }
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

    [HttpPut("editar/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> EditarUsuario(int id, [FromBody] UsuarioEdicionDTO dto)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound(new { success = false, message = "Usuario no encontrado.", data = (object)null });

        if (usuario.Correo != dto.Correo)
        {
            bool correoExiste = await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo && u.IdUsuario != id);
            if (correoExiste)
                return BadRequest(new { success = false, message = "El correo ya está en uso.", data = (object)null });

            usuario.Correo = dto.Correo;
        }

        usuario.Nombre = dto.Nombre;
        usuario.ApellidoPaterno = dto.ApellidoPaterno;
        usuario.ApellidoMaterno = dto.ApellidoMaterno;
        usuario.Edad = dto.Edad;
        usuario.Pais = dto.Pais;
        usuario.Telefono = dto.Telefono;

        if (!string.IsNullOrWhiteSpace(dto.Contrasenia))
            usuario.EstablecerPassword(dto.Contrasenia);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Cambios generados",
            data = new[]
            {
            new
            {
                usuario.IdUsuario,
                usuario.Nombre,
                usuario.ApellidoPaterno,
                usuario.ApellidoMaterno,
                usuario.Edad,
                usuario.Pais,
                usuario.Correo,
                Contrasenia = dto.Contrasenia,
                usuario.Telefono
            }
        }
        });
    }


    [HttpDelete("eliminar/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> EliminarUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound(new { success = false, message = "Usuario no encontrado.", data = (object)null });

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Usuario eliminado.", data = (object)null });
    }

}
