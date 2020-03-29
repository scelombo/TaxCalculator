using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaxCalculator.Models;

namespace TaxCalculator.Utilities
{
  public class Context : DbContext
  {
    IConfiguration _configuration;
    public Context(IConfiguration configuration) : base()
    {
      _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
    }
 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //modelBuilder.HasDefaultSchema("XYZ");
      //modelBuilder.Entity<Order>()
      //.HasIndex(o => new { o.field1, o.field2 }).IsUnique();
      base.OnModelCreating(modelBuilder);
    }
    public DbSet<Region> Regions { get; set; }
    public DbSet<CalculationType> CalculationTypes { get; set; }
    public DbSet<RegionTax> RegionTaxes { get; set; }
    public DbSet<Calculation> Calculations { get; set; }

  }
}

