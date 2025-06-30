namespace HidroSense.Data;
using Microsoft.EntityFrameworkCore;
using HidroSense.Models;

public class HidroSenseContext : DbContext
{
    public HidroSenseContext(DbContextOptions<HidroSenseContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<FuenteAgua> FuentesAgua { get; set; }
    public DbSet<Medicion> Mediciones { get; set; }
    public DbSet<SistemaPurificacion> SistemasPurificacion { get; set; }
    public DbSet<TratamientoAplicado> TratamientosAplicados { get; set; }
    public DbSet<ResultadoTratamiento> ResultadosTratamiento { get; set; }
    public DbSet<Alerta> Alertas { get; set; }
}
