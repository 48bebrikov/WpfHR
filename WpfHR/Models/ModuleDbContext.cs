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
            // Указываем путь к файлу базы данных в папке Database
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\AdaptationModulesDB.mdf;Integrated Security=True;");
        }
    }
}
