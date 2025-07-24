namespace HidroSense.DTOs
{
    public class ComponenteDTO
    {
        public string NombreComponente { get; set; }
        public string Descripcion { get; set; }
    }

    public class ProveedorConComponentesDTO
    {
        public string NombreProveedor { get; set; }
        public string NombreContacto { get; set; }
        public List<ComponenteDTO> Componentes { get; set; }
    }
}
