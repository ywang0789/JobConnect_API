using JobConnect_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace JobConnect_API.Data
{
    public class AppDbContext : IdentityDbContext<Account>
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Models.Application> Applications { get; set; }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Jobs
            modelBuilder.Entity<Job>().HasData(
                new Job
                {
                    job_id = 1,
                    title = "Software Engineer",
                    description = "Develop web applications.",
                    salary = 85000,
                    location = "New York"
                },
                new Job
                {
                    job_id = 2,
                    title = "Data Analyst",
                    description = "Analyze datasets and generate insights.",
                    salary = 65000,
                    location = "San Francisco"
                }
            );
        }

        public static async Task CreateTestUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<Account>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure roles exist
            string[] roles = { "recruiter", "applicant" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Recruiter account
            var recruiter = new Account
            {
                UserName = "recruiter@test.com",
                Email = "recruiter@test.com",
                first_name = "John",
                last_name = "Doe",
                role = "recruiter",
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(recruiter.Email) == null)
            {
                var result = await userManager.CreateAsync(recruiter, "Recruiter123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(recruiter, "recruiter");
                }
            }

            // Applicant account
            var applicant = new Account
            {
                UserName = "applicant@test.com",
                Email = "applicant@test.com",
                first_name = "Jane",
                last_name = "Smith",
                role = "applicant",
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(applicant.Email) == null)
            {
                var result = await userManager.CreateAsync(applicant, "Applicant123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(applicant, "applicant");
                }
            }
        }

    }
}
