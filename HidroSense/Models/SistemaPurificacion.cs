namespace HidroSense.Models;
using System.ComponentModel.DataAnnotations;

public class SistemaPurificacion
{
    [Key]
    public int IdSistema { get; set; }

    [Required]
    public string NombreSistema { get; set; }

    public string Descripcion { get; set; }

    public string NombreFabricante { get; set; }

    public double Precio { get; set; }

    public string UrlImagen { get; set; }
}


