using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HidroSense.Models
{
    public class Venta
    {
        [Key]
        public int IdVenta { get; set; }

        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        public Usuario Cliente { get; set; }

        [ForeignKey("Vendedor")]
        public int IdVendedor { get; set; }
        public Usuario Vendedor { get; set; }

        public DateTime FechaHora { get; set; }

        public ICollection<DetalleVenta> Detalles { get; set; }
    }
}
