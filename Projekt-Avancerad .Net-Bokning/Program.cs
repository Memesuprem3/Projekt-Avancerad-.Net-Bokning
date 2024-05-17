using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Projekt_Avancerad_.Net_Bokning.Data;
using Projekt_Avancerad_.Net_Bokning.Services;
using Microsoft.OpenApi.Models;

namespace Projekt_Avancerad_.Net_Bokning
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            // Repositories
            builder.Services.AddScoped<ICustomer, CustomerRepo>();
            builder.Services.AddScoped<ICompany, CompanyRepo>();
            builder.Services.AddScoped<IAppointment, AppointmentRepo>();

            // DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

            // Identity services
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // JWT Authentication
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // Authorization 
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireCustomerRole", policy => policy.RequireRole("Customer"));
                options.AddPolicy("RequireCompanyRole", policy => policy.RequireRole("Company"));
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            });

            var app = builder.Build();

            // Seed roles and users
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                string[] roleNames = { "Admin", "Customer", "Company" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Create a default Admin user
                var adminUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@example.com"
                };
                string adminPassword = "Admin@123";
                var user = await userManager.FindByEmailAsync(adminUser.Email);

                if (user == null)
                {
                    var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
                    if (createAdminUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }

                // Create additional users
                var customerUser = new IdentityUser
                {
                    UserName = "customer1",
                    Email = "customer1@example.com"
                };
                string customerPassword = "Customer@123";
                user = await userManager.FindByEmailAsync(customerUser.Email);

                if (user == null)
                {
                    var createCustomerUser = await userManager.CreateAsync(customerUser, customerPassword);
                    if (createCustomerUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(customerUser, "Customer");
                    }
                }

                var companyUser = new IdentityUser
                {
                    UserName = "company1",
                    Email = "company1@example.com"
                };
                string companyPassword = "Company@123";
                user = await userManager.FindByEmailAsync(companyUser.Email);

                if (user == null)
                {
                    var createCompanyUser = await userManager.CreateAsync(companyUser, companyPassword);
                    if (createCompanyUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(companyUser, "Company");
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}