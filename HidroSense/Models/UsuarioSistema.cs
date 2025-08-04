using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HidroSense.Models
{
    public class UsuarioSistema
    {
        [Key]
        public int IdUsuarioSistema { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        [ForeignKey("SistemaPurificacion")]
        public int IdSistema { get; set; }
        public SistemaPurificacion SistemaPurificacion { get; set; }
    }
}
