using CloudCityCakeCo.Data.EntityConfigurations;
using CloudCityCakeCo.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CloudCityCakeCo.Data
{
   
    public class ApplicationDbContext : IdentityDbContext<User, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

    
        public DbSet<CakeOrder> CakeOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            CakeOrderEntityConfiguration.Configure(modelBuilder);
           UserEntityConfiguration.Configure(modelBuilder);
        }
    }
}