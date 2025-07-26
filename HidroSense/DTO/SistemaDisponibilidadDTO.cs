using System.Collections.Generic;

namespace HidroSense.DTOs
{
    public class SistemaDisponibilidadDTO
    {
        public string NombreSistema { get; set; }
        public int CantidadDisponible { get; set; } // del modelo SistemaPurificacion

        public List<ComponenteRequeridoDTO> Componentes { get; set; }
    }

    public class ComponenteRequeridoDTO
    {
        public string NombreComponente { get; set; }
        public int CantidadRequerida { get; set; } // del modelo SistemaRequerimiento
        public int CantidadDisponible { get; set; } // del modelo ComponentesSistema
    }
}
