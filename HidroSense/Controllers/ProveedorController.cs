using HidroSense.Data;
using HidroSense.DTO;
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

        [HttpGet("componentes-por-proveedor")]
        public async Task<IActionResult> GetComponentesPorProveedor([FromHeader(Name = "idProveedor")] int idProveedor)
        {
            var componentes = await _context.ComponentesSistema
                .Where(c => c.IdProveedor == idProveedor)
                .Select(c => new
                {
                    c.IdComponente,
                    c.NombreComponente,
                    c.Cantidad,
                    c.Precio
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Componentes del proveedor obtenidos correctamente",
                data = componentes
            });
        }
        [HttpPut("actualizar-inventario")]
        public async Task<IActionResult> ActualizarInventario([FromBody] IngresoComponenteDTO dto)
        {
            var componente = await _context.ComponentesSistema.FindAsync(dto.IdComponente);

            if (componente == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Componente no encontrado"
                });
            }

            int cantidadAnterior = componente.Cantidad;
            decimal precioAnterior = componente.Precio;

            int cantidadTotal = cantidadAnterior + dto.CantidadAdquirida;

            // Calcular nuevo precio promedio
            decimal nuevoPrecio = ((precioAnterior * cantidadAnterior) + (dto.PrecioAdquisicion * dto.CantidadAdquirida)) / cantidadTotal;

            // Actualizar valores
            componente.Cantidad = cantidadTotal;
            componente.Precio = nuevoPrecio;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Inventario actualizado correctamente",
                data = new
                {
                    componente.IdComponente,
                    componente.NombreComponente,
                    componente.Cantidad,
                    componente.Precio
                }
            });
        }


    }

}
