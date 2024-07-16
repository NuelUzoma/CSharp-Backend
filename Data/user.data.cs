using Microsoft.EntityFrameworkCore;
using First_Backend.Models;


namespace First_Backend.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options): base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        // Utilizing Fluent API for unique constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Make username unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();
            
            // Make the email unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}