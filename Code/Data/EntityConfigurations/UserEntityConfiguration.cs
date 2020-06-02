using System;
using CloudCityCakeCo.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudCityCakeCo.Data.EntityConfigurations
{
    public class UserEntityConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(e => e.Id);
            });


        }
    }
}