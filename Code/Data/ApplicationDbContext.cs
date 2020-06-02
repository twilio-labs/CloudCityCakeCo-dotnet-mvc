using CloudCityCakeCo.Data.EntityConfigurations;
using CloudCityCakeCo.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudCityCakeCo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<CakeOrder> CakeOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           CakeOrderEntityConfiguration.Configure(modelBuilder);
           UserEntityConfiguration.Configure(modelBuilder);
        }
    }
}