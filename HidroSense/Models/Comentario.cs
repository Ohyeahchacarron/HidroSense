using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HidroSense.Models
{
    public class Comentario
    {
        [Key]
        public int IdComentario { get; set; }

        public string ComentarioTexto { get; set; }
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
    }
}
