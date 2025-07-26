using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HidroSense.Models
{
    public class SistemaRequerimiento
    {
        [Key]
        public int IdSistemaComponente { get; set; }

        [ForeignKey("SistemaPurificacion")]
        public int IdSistema { get; set; }
        public SistemaPurificacion SistemaPurificacion { get; set; }

        [ForeignKey("ComponentesSistema")]
        public int IdComponente { get; set; }
        public ComponentesSistema ComponentesSistema { get; set; }

        public int CantidadRequerida { get; set; }
    }
}
