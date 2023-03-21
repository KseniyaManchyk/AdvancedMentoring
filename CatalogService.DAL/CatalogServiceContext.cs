using CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.DAL;

public partial class CatalogServiceContext : DbContext
{
    public CatalogServiceContext()
    {
    }

    public CatalogServiceContext(DbContextOptions<CatalogServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // should be moved into config in the future
            optionsBuilder.UseSqlServer("Server=DESKTOP-3PKISU6;Initial Catalog=CatalogService;TrustServerCertificate=True;User ID=kseniya_manchyk;Password=050699;");
        }
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Image).HasMaxLength(2083);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Image).HasMaxLength(2083);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__267ABA7A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
