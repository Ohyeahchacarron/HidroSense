using HidroSense.Data;
using HidroSense.DTO;
using HidroSense.DTOs;
using HidroSense.Models;
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
                    IdProveedor= p.IdProveedor,
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

        [HttpPost("registrar-proveedor")]
        public async Task<IActionResult> RegistrarProveedorConComponentes([FromBody] ProveedorConComponentesDTO dto)
        {
            var proveedor = new Proveedor
            {
                NombreProveedor = dto.NombreProveedor,
                NombreContacto = dto.NombreContacto
            };

            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            var componentes = dto.Componentes.Select(c => new ComponentesSistema
            {
                NombreComponente = c.NombreComponente,
                Descripcion = c.Descripcion,
                Precio = c.Precio,
                Cantidad = c.Cantidad,
                IdProveedor = proveedor.IdProveedor
            }).ToList();

            _context.ComponentesSistema.AddRange(componentes);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Proveedor y componentes registrados correctamente",
                data = new
                {
                    proveedor.IdProveedor,
                    proveedor.NombreProveedor,
                    proveedor.NombreContacto,
                    Componentes = componentes.Select(c => new
                    {
                        c.IdComponente,
                        c.NombreComponente,
                        c.Descripcion,
                        c.Precio,
                        c.Cantidad
                    })
                }
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

            
            decimal nuevoPrecio = ((precioAnterior * cantidadAnterior) + (dto.PrecioAdquisicion * dto.CantidadAdquirida)) / cantidadTotal;

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
