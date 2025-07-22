using HidroSense.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ComponentesSistema
{
    [Key]
    public int IdComponente { get; set; }

    [ForeignKey("SistemaPurificacion")]
    public int IdSistema { get; set; }

    public SistemaPurificacion SistemaPurificacion { get; set; }

    public string NombreComponente { get; set; }
    public string Descripcion { get; set; }
    public double Precio { get; set; }
    public int Cantidad { get; set; }

    [ForeignKey("Proveedor")]
    public int IdProveedor { get; set; }

    public Proveedor Proveedor { get; set; }
}
