using MemberCoupon.Common;
using MemberCoupon.Data;
using MemberCoupon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MemberCoupon
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var sqliteConnStr = builder.Configuration.GetConnectionString("SqliteConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(sqliteConnStr));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddScoped<MemberService>();

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/");
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var admin = await userManager.FindByNameAsync("ccadmin");
                if (admin == null)
                {
                    await userManager.CreateAsync(new IdentityUser
                    {
                        UserName = "ccadmin",
                        Email = "hinleongwat@gmail.com",
                        EmailConfirmed = true
                    }, 
                    "PleaseChange");
                }

                var org = dbContext.Organizations.FirstOrDefault();
                if (org == null)
                {
                    dbContext.Organizations.Add(new Organization
                    {
                        Name = "Smiling Association",
                        PageHeader = "# Header"
                    });
                    dbContext.SaveChanges();
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}