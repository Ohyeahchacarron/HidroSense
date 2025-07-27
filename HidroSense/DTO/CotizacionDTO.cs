namespace HidroSense.DTO
{
    public class CrearCotizacionDTO
    {
        public string NombreContacto { get; set; }
        public string CorreoElectronico { get; set; }
        public string SituacionDetallada { get; set; }
    }

    public class CotizacionResumenDTO
    {
        public int IdCotizacion { get; set; }
        public string NombreContacto { get; set; }
        public string CorreoElectronico { get; set; }
        public string SituacionDetallada { get; set; }
    }

}
