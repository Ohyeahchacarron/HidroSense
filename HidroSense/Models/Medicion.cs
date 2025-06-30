namespace HidroSense.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Medicion
{
    [Key]
    public int IdMedicion { get; set; }

    [Required]
    public int IdFuente { get; set; }

    [ForeignKey("IdFuente")]
    public FuenteAgua FuenteAgua { get; set; }

    public DateTime FechaHora { get; set; }

    public float? Ph { get; set; }

    public float? NivelTurbidez { get; set; }

    public float? Temperatura { get; set; }
}

