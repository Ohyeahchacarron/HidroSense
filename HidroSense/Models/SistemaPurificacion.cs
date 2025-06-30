namespace HidroSense.Models;
using System.ComponentModel.DataAnnotations;

public class SistemaPurificacion
{
    [Key]
    public int IdSistema { get; set; }

    [Required]
    public string NombreSistema { get; set; }

    public string Descripcion { get; set; }
}


