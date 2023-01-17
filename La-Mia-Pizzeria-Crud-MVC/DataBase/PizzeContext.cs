using Microsoft.EntityFrameworkCore;
using La_Mia_Pizzeria_1.Models;
namespace La_Mia_Pizzeria_1.DataBase
{
    public class PizzeContext : DbContext
    {
        public DbSet<Pizza> Pizze { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Database=MiePizze1;" +
            "Integrated Security=True;TrustServerCertificate=True");
        }
    }
}
