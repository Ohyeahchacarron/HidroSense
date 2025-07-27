namespace HidroSense.Data;
using HidroSense.Models;
using Microsoft.EntityFrameworkCore;

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
    public DbSet<Comentario> Comentarios { get; set; }
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<DetalleVenta> DetallesVentas { get; set; }
    public DbSet<SistemaRequerimiento> SistemaRequerimientos { get; set; }
    public DbSet<UsuarioSistema> UsuarioSistemas { get; set; }
    public DbSet<Cotizacion> Cotizaciones { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Venta>()
            .HasOne(v => v.Cliente)
            .WithMany()
            .HasForeignKey(v => v.IdCliente)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Venta>()
            .HasOne(v => v.Vendedor)
            .WithMany()
            .HasForeignKey(v => v.IdVendedor)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SistemaRequerimiento>()
            .HasOne(sr => sr.SistemaPurificacion)
            .WithMany()
            .HasForeignKey(sr => sr.IdSistema)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SistemaRequerimiento>()
            .HasOne(sr => sr.ComponentesSistema)
            .WithMany()
            .HasForeignKey(sr => sr.IdComponente)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
