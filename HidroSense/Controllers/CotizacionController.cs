using HidroSense.Data;
using HidroSense.DTO;
using HidroSense.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;

namespace HidroSense.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CotizacionController : ControllerBase
    {
        private readonly HidroSenseContext _context;

        public CotizacionController(HidroSenseContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CrearCotizacion([FromBody] CrearCotizacionDTO dto)
        {
            var cotizacion = new Cotizacion
            {
                NombreContacto = dto.NombreContacto,
                CorreoElectronico = dto.CorreoElectronico,
                SituacionDetallada = dto.SituacionDetallada
            };

            _context.Cotizaciones.Add(cotizacion);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Cotización registrada correctamente",
                data = new { cotizacion.IdCotizacion }
            });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCotizaciones()
        {
            var cotizaciones = await _context.Cotizaciones
                .Select(c => new CotizacionResumenDTO
                {
                    IdCotizacion = c.IdCotizacion,
                    NombreContacto = c.NombreContacto,
                    CorreoElectronico = c.CorreoElectronico,
                    SituacionDetallada = c.SituacionDetallada
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Listado de cotizaciones",
                data = cotizaciones
            });
        }
    }

}
