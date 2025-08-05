namespace HidroSense.DTOs
{
    public class ComponenteDTO
    {
        public string NombreComponente { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }

    public class ProveedorConComponentesDTO
    {
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string NombreContacto { get; set; }
        public List<ComponenteDTO> Componentes { get; set; }
    }
}
