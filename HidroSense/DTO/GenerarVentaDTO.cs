using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HidroSense.DTOs
{
    public class DetalleVentaDTO
    {
        public int? IdSistema { get; set; }
        public int? IdComponente { get; set; }
        public int Cantidad { get; set; }
        public string? Nota { get; set; }
    }

    public class GenerarVentaDTO
    {
        public int IdCliente { get; set; }
        public int IdVendedor { get; set; }

        [JsonPropertyName("fechaVenta")] 
        public DateTime FechaHora { get; set; }

        public List<DetalleVentaDTO> Detalles { get; set; }
    }
}