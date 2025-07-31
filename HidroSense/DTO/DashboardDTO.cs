namespace HidroSense.DTOs
{
    public class DashboardDTO
    {
        public class ProductoPorMesDTO
        {
            public string Producto { get; set; }
            public int CantidadVendida { get; set; }
        }

        public class VentasPorVendedorDTO
        {
            public string Vendedor { get; set; }
            public decimal TotalVentas { get; set; }
        }

        public class VentasPorMesDTO
        {
            public string Mes { get; set; }
            public decimal TotalVentas { get; set; }
        }
    }
}
