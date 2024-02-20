using ChainGenerator.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChainGenerator.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<ChainGeneratorPageModel> ChainGeneratorPageModel { get; set; }
        public DbSet<WidgetGeneratorModel> WidgetGeneratorModel { get; set; }
        public DbSet<WidgetImageReferenceModel> ImageReferenceModel { get; set; }

        public override int SaveChanges()
        {
            HandleIUpdateable();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleIUpdateable();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void HandleIUpdateable()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is IUpdateable && (x.State == EntityState.Added || x.State == EntityState.Modified)).ToList();

            var currentDateTime = DateTime.UtcNow;

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((IUpdateable)entity.Entity).Created = currentDateTime;
                }

                ((IUpdateable)entity.Entity).LastUpdated = currentDateTime;
            }
        }
    }
}
