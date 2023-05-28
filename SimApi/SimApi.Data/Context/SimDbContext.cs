using Microsoft.EntityFrameworkCore;
using SimApi.Base;
using SimApi.Data;

namespace SimApi.Data.Context;

public class SimDbContext : DbContext
{
    public SimDbContext(DbContextOptions<SimDbContext> options) : base(options)
    {

    }

    // dbset
    public DbSet<Product> Product { get; set; }
    public DbSet<Category> Category { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        OnBeforeSaving();
        return base.SaveChanges();
    }
    private void OnBeforeSaving()
    {
        var entries = ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseModel trackable)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        trackable.CreatedAt = DateTime.UtcNow;
                        trackable.CreatedBy = "sim@sim.com";
                        break;

                    case EntityState.Modified:
                        trackable.UpdatedAt = DateTime.UtcNow;
                        trackable.UpdatedBy = "sim@sim.com";
                        entry.Property("CreatedAt").IsModified = false;
                        entry.Property("CreatedBy").IsModified = false;
                        break;
                }
            }
        }
    }

}
