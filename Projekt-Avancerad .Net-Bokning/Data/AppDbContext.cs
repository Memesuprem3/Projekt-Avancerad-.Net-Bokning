using Microsoft.EntityFrameworkCore;
using Projekt_Models;

namespace Projekt_Avancerad_.Net_Bokning.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<Customer> customer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //SeedData




            modelBuilder.Entity<Appointment>().HasData(new Appointment { });


        }
    }
}
