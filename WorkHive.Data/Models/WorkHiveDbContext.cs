using Microsoft.EntityFrameworkCore;
using WorkHive.Data.Models.Entities;
using System.Reflection;

namespace WorkHive.Data.Models
{
    public class WorkHiveDbContext : DbContext
    {
        public WorkHiveDbContext(DbContextOptions<WorkHiveDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
    }
}
