using HidroSense.Data;
using HidroSense.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//[Authorize]
[ApiController]
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
        medicion.NivelTurbidez = dto.NivelTurbidez;
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

}
