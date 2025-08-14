using Microsoft.EntityFrameworkCore;
using MoustafaTasks.Domain;
using MoustafaTasks.Infrastructure.FluentAPIConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoustafaTasks.Infrastructure;

public class TestingDbContext : DbContext
{
   public DbSet<SecUser> SecUsers { get; set; }
   public DbSet<Device> Devices { get; set; }   

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=10.10.61.44;Database=TestingI;user id=sa;password=W@tcher123@itlink2233;MultipleActiveResultSets=true;TrustServerCertificate=True");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SecUserConfig).Assembly);    
    }
}
