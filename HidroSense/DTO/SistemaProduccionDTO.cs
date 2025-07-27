namespace HidroSense.DTO
{
    public class SistemaProduccionDTO
    {
        public string NombreSistema { get; set; }
        public int SistemasDisponibles { get; set; }
        public List<ComponenteSimpleDTO> Componentes { get; set; }
        public decimal CostoTotalProduccion { get; set; }
    }

    public class ComponenteSimpleDTO
    {
        public string NombreComponente { get; set; }
        public int CantidadRequerida { get; set; }
        public int CantidadDisponible { get; set; }
        
    }

}
