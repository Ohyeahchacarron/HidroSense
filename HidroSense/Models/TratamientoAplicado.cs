namespace HidroSense.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TratamientoAplicado
{
    [Key]
    public int IdTratamiento { get; set; }

    public int IdFuente { get; set; }

    [ForeignKey("IdFuente")]
    public FuenteAgua FuenteAgua { get; set; }

    public int IdSistema { get; set; }

    [ForeignKey("IdSistema")]
    public SistemaPurificacion SistemaPurificacion { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public string Observaciones { get; set; }
}

