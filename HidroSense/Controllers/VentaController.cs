using HidroSense.Data;
using HidroSense.DTOs;
using HidroSense.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HidroSense.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VentaController : ControllerBase
    {
        private readonly HidroSenseContext _context;

        public VentaController(HidroSenseContext context)
        {
            _context = context;
        }

        [HttpGet("clientes")] 
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _context.Usuarios
                .Where(u => u.Nivel == "2") 
                .Select(u => new { u.IdUsuario, u.Nombre, u.ApellidoPaterno, u.ApellidoMaterno, u.Correo })
                .ToListAsync();

            return Ok(new { success = true, message = "Clientes obtenidos", data = clientes });
        }

        [HttpGet("productos-venta")]
        public async Task<IActionResult> GetProductosParaVenta()
        {
            var sistemas = await _context.SistemasPurificacion
                .Select(s => new { Id = s.IdSistema, Nombre = s.NombreSistema, Tipo = "sistema", CantidadDisponible = s.Cantidad })
                .ToListAsync();

            var componentes = await _context.ComponentesSistema
                .Select(c => new { Id = c.IdComponente, Nombre = c.NombreComponente, Tipo = "componente", CantidadDisponible = c.Cantidad })
                .ToListAsync();

            var productos = new List<object>();
            productos.AddRange(sistemas);
            productos.AddRange(componentes);

            return Ok(new { success = true, message = "Productos para venta obtenidos", data = productos });
        }
        [HttpPost("venta")]
        public async Task<IActionResult> RegistrarVenta([FromBody] GenerarVentaDTO dto)
        {
            try
            {
                foreach (var detalleDto in dto.Detalles)
                {
                    if (detalleDto.IdComponente.HasValue)
                    {
                        var componente = await _context.ComponentesSistema.FindAsync(detalleDto.IdComponente.Value);
                        if (componente == null)
                            return NotFound(new { success = false, message = "Componente no encontrado" });

                        if (componente.Cantidad < detalleDto.Cantidad)
                            return BadRequest(new { success = false, message = $"Stock insuficiente del componente con ID {detalleDto.IdComponente}" });
                    }
                    else if (detalleDto.IdSistema.HasValue)
                    {
                        var sistema = await _context.SistemasPurificacion.FindAsync(detalleDto.IdSistema.Value);
                        if (sistema == null)
                            return NotFound(new { success = false, message = "Sistema no encontrado" });

                        if (sistema.Cantidad < detalleDto.Cantidad)
                            return BadRequest(new { success = false, message = $"Stock insuficiente del sistema con ID {detalleDto.IdSistema}" });
                    }
                }

                var venta = new Venta
                {
                    IdCliente = dto.IdCliente,
                    IdVendedor = dto.IdVendedor,
                    FechaHora = dto.FechaHora,
                    Detalles = new List<DetalleVenta>()
                };

                foreach (var detalleDto in dto.Detalles)
                {
                    decimal costoBase = 0;

                    if (detalleDto.IdComponente.HasValue)
                    {
                        var componente = await _context.ComponentesSistema.FindAsync(detalleDto.IdComponente.Value);

                        costoBase = componente.Precio;
                        componente.Cantidad -= detalleDto.Cantidad;
                    }
                    else if (detalleDto.IdSistema.HasValue)
                    {
                        var sistema = await _context.SistemasPurificacion.FindAsync(detalleDto.IdSistema.Value);

                        var requerimientos = await _context.SistemaRequerimientos
                            .Where(sr => sr.IdSistema == sistema.IdSistema)
                            .Include(sr => sr.ComponentesSistema)
                            .ToListAsync();

                        foreach (var req in requerimientos)
                        {
                            costoBase += req.ComponentesSistema.Precio * req.CantidadRequerida;
                        }

                        sistema.Cantidad -= detalleDto.Cantidad;

                    }

                    decimal total = Math.Round(costoBase * detalleDto.Cantidad * 1.3M, 2);

                    venta.Detalles.Add(new DetalleVenta
                    {
                        IdComponente = detalleDto.IdComponente,
                        IdSistema = detalleDto.IdSistema,
                        Cantidad = detalleDto.Cantidad,
                        Nota = detalleDto.Nota,
                        Total = total
                    });
                }

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Venta registrada exitosamente",
                    data = new { venta.IdVenta }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Error al registrar la venta",
                    error = ex.Message
                });
            }
        }

    }
}
