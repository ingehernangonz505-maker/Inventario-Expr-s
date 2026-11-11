using ControlInventarioExpress.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlInventarioExpress.Data;

public class InventarioDbContext(DbContextOptions<InventarioDbContext> options)
    : DbContext(options)
{
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.ToTable("proveedor");
            entity.HasIndex(e => e.Ruc).IsUnique();
            entity.HasIndex(e => e.Correo).IsUnique();
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("producto");
            entity.Property(p => p.PrecioCompra).HasPrecision(10, 2);
            entity.Property(p => p.PrecioVenta).HasPrecision(10, 2);

            entity.HasOne(p => p.Proveedor)
                  .WithMany(p => p.Productos)
                  .HasForeignKey(p => p.ProveedorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
