using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projekt_Models;
using System.Reflection.Emit;

namespace Projekt_Avancerad_.Net_Bokning.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Customer> customer { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<BookingHistory> BookingHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User - Appointment relationship
            builder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId);

            // Customer - Appointment relationship
            builder.Entity<Customer>()
                .HasMany(c => c.Appointments)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId);

            // Company - Appointment relationship
            builder.Entity<Company>()
                .HasMany(c => c.Appointments)
                .WithOne(a => a.Company)
                .HasForeignKey(a => a.CompanyId);

            // BookingHistory relationship
            builder.Entity<BookingHistory>()
                .HasOne(bh => bh.Appointment)
                .WithMany(a => a.BookingHistories)
                .HasForeignKey(bh => bh.AppointmentId);

            // Seed data for roles
            builder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 2, Name = "Customer", NormalizedName = "CUSTOMER" },
                new IdentityRole<int> { Id = 3, Name = "Company", NormalizedName = "COMPANY" }
            );

            // Seed data for users
            var hasher = new PasswordHasher<User>();

            var admin = new User
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@123"),
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = "Admin",
                IsActive = true
            };

            var customer = new User
            {
                Id = 2,
                UserName = "customer1",
                NormalizedUserName = "CUSTOMER1",
                Email = "customer1@example.com",
                NormalizedEmail = "CUSTOMER1@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Customer@123"),
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = "Customer",
                IsActive = true
            };

            var company = new User
            {
                Id = 3,
                UserName = "company1",
                NormalizedUserName = "COMPANY1",
                Email = "company1@example.com",
                NormalizedEmail = "COMPANY1@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Company@123"),
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = "Company",
                IsActive = true
            };

            builder.Entity<User>().HasData(admin, customer, company);

            // Assign roles to users
            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { UserId = 1, RoleId = 1 },
                new IdentityUserRole<int> { UserId = 2, RoleId = 2 },
                new IdentityUserRole<int> { UserId = 3, RoleId = 3 }
            );

            builder.Entity<Customer>().HasData(new Customer
            {
                CustomerId = 1,
                FristName = "Anna",
                LastName = "Svensson",
                Adress = "1234 Main St",
                Phone = "123-456-7890",
                Email = "annaecool@hotmail.com"
            });
            builder.Entity<Customer>().HasData(new Customer
            {
                CustomerId = 2,
                FristName = "Jonas",
                LastName = "Hellqvist",
                Adress = "Vivolvägen 12",
                Phone = "7778889932",
                Email = "R41nFire@hotmail.com"
            });
            builder.Entity<Customer>().HasData(new Customer
            {
                CustomerId = 3,
                FristName = "Stefan",
                LastName = "Magnusson",
                Adress = "Hästhagen 3",
                Phone = "7778889932",
                Email = "bilarebra@hotmail.com"
            });
            builder.Entity<Customer>().HasData(new Customer
            {
                CustomerId = 4,
                FristName = "Ronny",
                LastName = "Ronnysson",
                Adress = "Hagelbrakare 41",
                Phone = "7778889932",
                Email = "fettmedraggarvalle@hotmail.com"
            });
            builder.Entity<Customer>().HasData(new Customer
            {
                CustomerId = 5,
                FristName = "Ragge",
                LastName = "Raggesson",
                Adress = "Hagelbrakare 42",
                Phone = "7778889932",
                Email = "fettmedraggarvalle111@hotmail.com"
            });


            builder.Entity<Company>().HasData(new Company
            {
                CompanyId = 1,
                CompanyName = "SaabParts"

            });
            builder.Entity<Company>().HasData(new Company
            {
                CompanyId = 2,
                CompanyName = "VoloParts"

            });
            builder.Entity<Company>().HasData(new Company
            {
                CompanyId = 3,
                CompanyName = "Macken"

            });



            builder.Entity<Appointment>().HasData(new Appointment
            {
                id = 1,
                AppointDiscription = "Initial Consultation",
                PlacedApp = new DateTime(2011, 06, 22),
                CustomerId = 1,
                CompanyId = 1,
                UserId = 1 // Updated UserId
            });

            builder.Entity<Appointment>().HasData(new Appointment
            {
                id = 2,
                AppointDiscription = "Second Consultation",
                PlacedApp = new DateTime(2011, 06, 29),
                CustomerId = 1,
                CompanyId = 1,
                UserId = 1 // Updated UserId
            });

            builder.Entity<Appointment>().HasData(new Appointment
            {
                id = 3,
                AppointDiscription = "Third Consultation",
                PlacedApp = new DateTime(2011, 07, 06),
                CustomerId = 1,
                CompanyId = 1,
                UserId = 1 // Updated UserId
            });

            builder.Entity<Appointment>().HasData(new Appointment
            {
                id = 4,
                AppointDiscription = "Initial Consultation",
                PlacedApp = new DateTime(2011, 06, 22),
                CustomerId = 2,
                CompanyId = 2,
                UserId = 2 // Updated UserId
            });
        }
    }
}