using HidroSense.Data;
using HidroSense.DTOs;
using HidroSense.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HidroSense.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SistemaController : ControllerBase
    {
        private readonly HidroSenseContext _context;

        public SistemaController(HidroSenseContext context)
        {
            _context = context;
        }

        [HttpGet("catalogo")]
        public IActionResult ObtenerCatalogo()
        {
            var sistemas = _context.SistemasPurificacion
                .Select(s => new SistemaPurificacionCatalogoDTO
                {
                    NombreSistema = s.NombreSistema,
                    UrlImagen = s.UrlImagen
                })
                .ToList();

            return Ok(new
            {
                success = true,
                message = "Catálogo obtenido correctamente",
                data = sistemas
            });
        }

        [HttpGet("descripcion/{idSistema}")]
        public async Task<IActionResult> ObtenerSistemaConComponentes(int idSistema)
        {
            var sistema = await _context.SistemasPurificacion
                .Where(s => s.IdSistema == idSistema)
                .Select(s => new ComponentesDTO
                {
                    NombreSistema = s.NombreSistema,
                    NombreFabricante = s.NombreFabricante,
                    UrlImagen = s.UrlImagen,
                    Componentes = _context.ComponentesSistema
                        .Where(c => c.IdSistema == idSistema)
                        .Select(c => c.NombreComponente)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (sistema == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Sistema no encontrado",
                    data = (object)null
                });
            }

            return Ok(new
            {
                success = true,
                message = "Sistema con componentes encontrado",
                data = sistema
            });
        }
        [HttpGet("comentarios")]
        public async Task<IActionResult> ObtenerComentarios()
        {
            var comentarios = await _context.Comentarios
                .Include(c => c.Usuario)
                .Select(c => new ComentarioDTO
                {
                    NombreUsuario = c.Usuario.Nombre + " " + c.Usuario.ApellidoPaterno + " " + c.Usuario.ApellidoMaterno,
                    NombreSistema = _context.SistemasPurificacion
                        .Where(s => s.IdUsuario == c.IdUsuario)
                        .Select(s => s.NombreSistema)
                        .FirstOrDefault(),
                    Comentario = c.ComentarioTexto
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Lista de comentarios",
                data = comentarios
            });
        }

    }
}
