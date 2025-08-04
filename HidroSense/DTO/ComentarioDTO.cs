namespace HidroSense.DTOs
{
    public class ComentarioDTO
    {
        public string NombreUsuario { get; set; }
        public string NombreSistema { get; set; }
        public string Comentario { get; set; }
    }

    public class ReviewDTO
    {
        public string ComentarioTexto { get; set; }
        public int IdUsuario { get; set; }
    }
    public class ResponderComentarioDTO 
    {
        public int IdComentario { get; set; } 
        public string RespuestaTexto { get; set; } 
    }
}
