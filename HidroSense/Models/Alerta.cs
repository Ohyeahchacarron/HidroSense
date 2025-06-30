namespace HidroSense.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Alerta
{
    [Key]
    public int IdAlerta { get; set; }

    public int IdFuente { get; set; }

    [ForeignKey("IdFuente")]
    public FuenteAgua FuenteAgua { get; set; }

    public DateTime FechaHora { get; set; }

    public string TipoAlerta { get; set; }

    public string Mensaje { get; set; }
}

