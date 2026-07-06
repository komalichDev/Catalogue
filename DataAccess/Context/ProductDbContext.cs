using Common.Types;
using DatabaseAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess.Context;

public class ProductDbContext : DbContext, IProductDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Description> Descriptions { get; set; }

    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
                    .Property(e => e.Id)
                    .HasConversion(v => v.Value, db => CategoryId.From(db));

        modelBuilder.Entity<Description>()
            .Property(e => e.Id)
            .HasConversion(v => v.Value, db => DescriptionId.From(db));

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id)
                  .HasConversion(v => v.Value, db => ProductId.From(db));

            entity.Property(e => e.CategoryId)
                  .HasConversion(v => v.Value, db => CategoryId.From(db));

            entity.Property(e => e.DescriptionId)
                  .HasConversion(v => v.Value, db => DescriptionId.From(db));
        });
    }
}
