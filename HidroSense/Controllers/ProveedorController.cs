using HidroSense.Data;
using HidroSense.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HidroSense.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProveedorController : ControllerBase
    {
        private readonly HidroSenseContext _context;

        public ProveedorController(HidroSenseContext context)
        {
            _context = context;
        }

        [HttpGet("proveedores_componentes")]
        public async Task<IActionResult> ObtenerProveedoresConComponentes()
        {
            var proveedores = await _context.Proveedores
                .Select(p => new ProveedorConComponentesDTO
                {
                    NombreProveedor = p.NombreProveedor,
                    NombreContacto = p.NombreContacto,
                    Componentes = _context.ComponentesSistema
                        .Where(c => c.IdProveedor == p.IdProveedor)
                        .Select(c => new ComponenteDTO
                        {
                            NombreComponente = c.NombreComponente,
                            Descripcion = c.Descripcion
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Proveedores con sus componentes",
                data = proveedores
            });
        }
    }
}
