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

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarVenta([FromBody] GenerarVentaDTO dto)
        {
            try
            {
                var venta = new Venta
                {
                    IdCliente = dto.IdCliente,
                    IdVendedor = dto.IdVendedor,
                    FechaHora = dto.FechaHora,
                    Detalles = new List<DetalleVenta>()
                };

                foreach (var detalleDto in dto.Detalles)
                {
                    var detalle = new DetalleVenta
                    {
                        IdSistema = detalleDto.IdSistema,
                        IdComponente = detalleDto.IdComponente,
                        Cantidad = detalleDto.Cantidad,
                        Nota = detalleDto.Nota
                    };
                    venta.Detalles.Add(detalle);
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
