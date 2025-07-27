using HidroSense.Data;
using HidroSense.DTO;
using HidroSense.DTOs;
using HidroSense.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HidroSense.Controllers
{
    //[Authorize]
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
                    Componentes = _context.SistemaRequerimientos
                        .Where(sr => sr.IdSistema == idSistema)
                        .Select(sr => sr.ComponentesSistema.NombreComponente)
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
                    NombreSistema = _context.UsuarioSistemas
                        .Where(us => us.IdUsuario == c.IdUsuario)
                        .Join(_context.SistemasPurificacion,
                            us => us.IdSistema,
                            s => s.IdSistema,
                            (us, s) => s.NombreSistema)
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

        [HttpGet("sistemas-produccion")]
        public async Task<IActionResult> ObtenerSistemasConComponentes()
        {
            var sistemas = await _context.SistemasPurificacion.ToListAsync();
            var resultado = new List<SistemaProduccionDTO>();

            foreach (var sistema in sistemas)
            {
                var requerimientos = await _context.SistemaRequerimientos
                    .Where(r => r.IdSistema == sistema.IdSistema)
                    .Include(r => r.ComponentesSistema)
                    .ToListAsync();

                var componentesDTO = new List<ComponenteSimpleDTO>();
                decimal costoTotal = 0;

                foreach (var req in requerimientos)
                {
                    var componente = req.ComponentesSistema;
                    if (componente != null)
                    {
                        costoTotal += componente.Precio * req.CantidadRequerida;

                        componentesDTO.Add(new ComponenteSimpleDTO
                        {
                            NombreComponente = componente.NombreComponente,
                            CantidadRequerida = req.CantidadRequerida,
                            CantidadDisponible = componente.Cantidad
                        });
                    }
                }

                resultado.Add(new SistemaProduccionDTO
                {
                    NombreSistema = sistema.NombreSistema,
                    SistemasDisponibles = sistema.Cantidad,
                    Componentes = componentesDTO,
                    CostoTotalProduccion = costoTotal
                });
            }

            return Ok(new
            {
                success = true,
                message = "Sistemas con componentes y costo total obtenidos.",
                data = resultado
            });
        }

        [HttpGet("inventario")]
        public async Task<IActionResult> ObtenerInventarioComponentes()
        {
            var componentes = await _context.ComponentesSistema
                .Select(c => new ComponenteInventarioDTO
                {
                    NombreComponente = c.NombreComponente,
                    Cantidad = c.Cantidad,
                    Precio = c.Precio
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Inventario de componentes",
                data = componentes
            });
        }

    }
}
