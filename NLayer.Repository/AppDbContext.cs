using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;

namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }

        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entitiyReference)
                {
                    switch (item.Entity)
                    {
                        case EntityState.Added:
                            {
                                entitiyReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                entitiyReference.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var item in ChangeTracker.Entries())
            {
                if(item.Entity is BaseEntity entitiyReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entitiyReference.CreatedDate = DateTime.Now;
                                break;
                            }
                            case EntityState.Modified:
                            {
                                Entry(entitiyReference).Property(x => x.CreatedDate).IsModified = false;
                                entitiyReference.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }

       
    }
}