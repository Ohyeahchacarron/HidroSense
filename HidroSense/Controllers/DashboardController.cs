using HidroSense.Data;
using HidroSense.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HidroSense.DTOs.DashboardDTO;

namespace HidroSense.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly HidroSenseContext _context;

        public DashboardController(HidroSenseContext context)
        {
            _context = context;
        }

        [HttpGet("productos-por-mes")]
        public async Task<IActionResult> GetProductosPorMes([FromQuery] int mes, [FromQuery] int anio)
        {
            var productos = await _context.DetallesVentas
                .Where(d => d.Venta.FechaHora.Month == mes && d.Venta.FechaHora.Year == anio)
                .Select(d => new
                {
                    Nombre = d.IdSistema != null ? d.SistemaPurificacion.NombreSistema :
                             d.IdComponente != null ? d.ComponentesSistema.NombreComponente :
                             "Sin nombre",
                    d.Cantidad
                })
                .GroupBy(x => x.Nombre)
                .Select(g => new ProductoPorMesDTO
                {
                    Producto = g.Key,
                    CantidadVendida = g.Sum(x => x.Cantidad)
                })
                .ToListAsync();

            return Ok(new { success = true, message = "OK", data = productos });
        }

        [HttpGet("ventas-por-vendedor")]
        public async Task<IActionResult> GetVentasPorVendedor([FromQuery] int mes, [FromQuery] int anio)
        {
            var ventas = await _context.Ventas
                .Where(v => v.FechaHora.Month == mes && v.FechaHora.Year == anio)
                .SelectMany(v => v.Detalles.Select(d => new
                {
                    Vendedor = v.Vendedor.Nombre + " " + v.Vendedor.ApellidoPaterno,
                    d.Total
                }))
                .GroupBy(x => x.Vendedor)
                .Select(g => new VentasPorVendedorDTO
                {
                    Vendedor = g.Key,
                    TotalVentas = g.Sum(x => x.Total)
                })
                .ToListAsync();

            return Ok(new { success = true, message = "OK", data = ventas });
        }

        [HttpGet("ventas-por-mes")]
        public async Task<IActionResult> GetVentasPorMes([FromQuery] int anio)
        {
            var ventas = await _context.Ventas
                .Where(v => v.FechaHora.Year == anio)
                .SelectMany(v => v.Detalles.Select(d => new
                {
                    Mes = v.FechaHora.Month,
                    d.Total
                }))
                .GroupBy(x => x.Mes)
                .Select(g => new VentasPorMesDTO
                {
                    Mes = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    TotalVentas = g.Sum(x => x.Total)
                })
                .OrderBy(x => DateTime.ParseExact(x.Mes, "MMMM", null).Month)
                .ToListAsync();

            return Ok(new { success = true, message = "OK", data = ventas });
        }
    }
}
