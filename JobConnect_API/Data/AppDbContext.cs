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

            // Seed Accounts
            var hasher = new PasswordHasher<Account>();

            var recruiter = new Account
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "recruiter@example.com",
                NormalizedUserName = "RECRUITER@EXAMPLE.COM",
                Email = "recruiter@example.com",
                NormalizedEmail = "RECRUITER@EXAMPLE.COM",
                first_name = "John",
                last_name = "Doe",
                role = "recruiter",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            recruiter.PasswordHash = hasher.HashPassword(recruiter, "Recruiter123!");

            var applicant = new Account
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "applicant@example.com",
                NormalizedUserName = "APPLICANT@EXAMPLE.COM",
                Email = "applicant@example.com",
                NormalizedEmail = "APPLICANT@EXAMPLE.COM",
                first_name = "Jane",
                last_name = "Smith",
                role = "applicant",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            applicant.PasswordHash = hasher.HashPassword(applicant, "Applicant123!");

            modelBuilder.Entity<Account>().HasData(recruiter, applicant);

            // Seed Applications
            modelBuilder.Entity<Models.Application>().HasData(
                new Models.Application
                {
                    application_id = 1,
                    job_id = 1,
                    account_id = applicant.Id,
                    content = "Hire me plz :pray:",
                    status = "Pending"
                }
            );
        }

    }
}
