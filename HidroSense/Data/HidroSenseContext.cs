namespace HidroSense.Data;
using HidroSense.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

public class HidroSenseContext : DbContext
{
    public HidroSenseContext(DbContextOptions<HidroSenseContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<FuenteAgua> FuentesAgua { get; set; }
    public DbSet<Medicion> Mediciones { get; set; }
    public DbSet<SistemaPurificacion> SistemasPurificacion { get; set; }
    public DbSet<Alerta> Alertas { get; set; }
    public DbSet<ComponentesSistema> ComponentesSistema { get; set; }
    public DbSet<Proveedor> Proveedores { get; set; }
}

