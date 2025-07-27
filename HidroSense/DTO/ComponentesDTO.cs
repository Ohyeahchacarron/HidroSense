namespace HidroSense.DTOs
{
    public class ComponentesDTO
    {
        public string NombreSistema { get; set; }
        public string NombreFabricante { get; set; }
        public string UrlImagen { get; set; }
        public List<string> Componentes { get; set; }
    }
    public class ComponenteInventarioDTO
    {
        public string NombreComponente { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
