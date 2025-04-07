using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Entities.EmlTask> Tasks { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Entities.EmlTask>().ToTable("Tasks");
            base.OnModelCreating(builder);
        }
    }
}
