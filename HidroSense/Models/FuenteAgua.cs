namespace HidroSense.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class FuenteAgua
{
    [Key]
    public int IdFuente { get; set; }

    [Required]
    public string NombreFuente { get; set; }

    public string TipoFuente { get; set; }

    public string Descripcion { get; set; }

    public decimal Latitud { get; set; }

    public decimal Altitud { get; set; }
}

