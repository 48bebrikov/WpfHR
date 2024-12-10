using Microsoft.EntityFrameworkCore;

namespace WpfHR.Models
{
    public class ModuleDbContext : DbContext
    {
        public DbSet<Module> Modules { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AdaptationProgram> AdaptationPrograms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AdaptationModulesDB;Trusted_Connection=True;");
        }
    }
}
