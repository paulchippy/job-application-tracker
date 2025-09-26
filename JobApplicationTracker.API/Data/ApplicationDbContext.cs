using JobApplicationTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobApplication>().Property(e => e.Status).HasConversion<string>();
        }
    }
}
