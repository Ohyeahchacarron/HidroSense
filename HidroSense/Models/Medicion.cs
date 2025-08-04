namespace HidroSense.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Medicion
{
    [Key]
    public int IdMedicion { get; set; }
    public DateTime FechaHora { get; set; }

    public float? Ph { get; set; }

    public float? NivelTurbidez { get; set; }

    public float? Temperatura { get; set; }

    [ForeignKey ("FuenteAgua")]
    public int IdFuente { get; set; }

    public FuenteAgua FuenteAgua { get; set; }

    [ForeignKey ("UsuarioSistema")]
    public int IdUsuarioSistema { get; set; }
    public UsuarioSistema UsuarioSistema { get; set; }
}

