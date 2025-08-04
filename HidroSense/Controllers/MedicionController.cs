using HidroSense.Data;
using HidroSense.DTOs;
using HidroSense.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MedicionesController : ControllerBase
{
    private readonly HidroSenseContext _context;

    public MedicionesController(HidroSenseContext context)
    {
        _context = context;
    }

    [HttpGet("sensores")]
    public async Task<IActionResult> ObtenerSensores()
    {
        var mediciones = await _context.Mediciones
            .OrderByDescending(m => m.FechaHora)
            .Select(m => new
            {
                idSensor = m.IdMedicion,
                ph = m.Ph,
                temp = m.Temperatura,
                turbidez = m.NivelTurbidez
            })
            .ToListAsync();

        return Ok(new
        {
            success = true,
            message = "Lectura obtenida",
            data = new { sensores = mediciones }
        });
    }

    [HttpPut("actualizar/{id}")]
    public async Task<IActionResult> ActualizarMedicion(int id, [FromBody] MedicionDTO dto)
    {
        var medicion = await _context.Mediciones.FindAsync(id);
        if (medicion == null)
            return NotFound(new { success = false, message = "Medición no encontrada", data = (object)null });

        medicion.Ph = dto.Ph;
        medicion.NivelTurbidez = dto.Turbidez;
        medicion.Temperatura = dto.Temperatura;
        medicion.FechaHora = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Medición actualizada",
            data = new
            {
                idSensor = medicion.IdFuente,
                ph = medicion.Ph,
                temp = medicion.Temperatura,
                turbidez = medicion.NivelTurbidez
            }
        });
    }

    [HttpGet("usuario/{idUsuario}")]
    public async Task<IActionResult> ObtenerMedicionesPorUsuario(int idUsuario)
    {
        
        var datos = await _context.Mediciones
            .Include(m => m.UsuarioSistema) 
                .ThenInclude(us => us.SistemaPurificacion) 
            .Include(m => m.FuenteAgua) 
            .Where(m => m.UsuarioSistema.IdUsuario == idUsuario) 
            .Select(m => new
            {
                nombreSistema = m.UsuarioSistema.SistemaPurificacion.NombreSistema, 
                nombreFuente = m.FuenteAgua.NombreFuente, 
                ph = m.Ph,
                turbidez = m.NivelTurbidez, 
                temperatura = m.Temperatura
            })
            .ToListAsync();

        if (datos == null || !datos.Any())
        {
            return Ok(new
            {
                success = true,
                message = "No se encontraron lecturas para el usuario.",
                data = new
                {
                    sensores = new List<object>() 
                }
            });
        }

        return Ok(new
        {
            success = true,
            message = "Lectura obtenida",
            data = new
            {
                sensores = datos
            }
        });
    
}

}