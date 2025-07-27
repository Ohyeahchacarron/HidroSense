using System.ComponentModel.DataAnnotations;

namespace HidroSense.Models
{
    public class Cotizacion
    {
        [Key]
        public int IdCotizacion { get; set; }
        public string NombreContacto { get; set; }
        public string CorreoElectronico { get; set; }
        public string SituacionDetallada { get; set; }
    }

}
