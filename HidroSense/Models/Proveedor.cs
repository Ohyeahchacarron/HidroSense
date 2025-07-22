using System.ComponentModel.DataAnnotations;

namespace HidroSense.Models
{
    public class Proveedor
    {
        [Key]
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }

        public string NombreContacto { get; set; }
      

    }
}
