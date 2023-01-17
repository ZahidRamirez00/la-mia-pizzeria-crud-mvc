﻿using Microsoft.EntityFrameworkCore;
using La_Mia_Pizzeria_Crud_MVC.Models;
namespace La_Mia_Pizzeria_Crud_MVC.DataBase
{
    public class PizzeContext : DbContext
    {
        public DbSet<Pizza> Pizze { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Database=MiePizze_2_;" +
            "Integrated Security=True;TrustServerCertificate=True");
        }
    }
}
