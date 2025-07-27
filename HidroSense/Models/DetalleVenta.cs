using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HidroSense.Models
{
    public class DetalleVenta
    {
        [Key]
        public int IdDetalleVenta { get; set; }

        [ForeignKey("Venta")]
        public int IdVenta { get; set; }
        public Venta Venta { get; set; }

        [ForeignKey("SistemaPurificacion")]
        public int? IdSistema { get; set; }
        public SistemaPurificacion? SistemaPurificacion { get; set; }

        [ForeignKey("ComponentesSistema")]
        public int? IdComponente { get; set; }
        public ComponentesSistema? ComponentesSistema { get; set; }

        public int Cantidad { get; set; }

        public string? Nota { get; set; }
        public decimal Total { get; set; }
    }
}
