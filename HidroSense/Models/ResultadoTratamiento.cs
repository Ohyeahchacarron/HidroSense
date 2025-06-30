namespace HidroSense.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ResultadoTratamiento
{
    [Key]
    public int IdResultado { get; set; }

    public int IdTratamiento { get; set; }

    [ForeignKey("IdTratamiento")]
    public TratamientoAplicado TratamientoAplicado { get; set; }

    public DateTime Fecha { get; set; }

    public float? PhFinal { get; set; }

    public float? TurbidezFinal { get; set; }

    public float? TemperaturaFinal { get; set; }

    public string Observaciones { get; set; }
}

