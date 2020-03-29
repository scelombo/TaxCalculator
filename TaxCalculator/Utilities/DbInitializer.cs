using Serilog;
using System.Collections.Generic;
using System.Linq;
using TaxCalculator.Models;

namespace TaxCalculator.Utilities
{
  public static partial class DbInitializer
  {
    private static Context _context;
    private static ILogger _logger;
    public static void Initialize(Context context, ILogger logger)
    {
      _context = context;
      _logger = logger;

      if (!context.CalculationTypes.Any())
        SeedCalculationTypes();

      if (!context.Regions.Any())
        SeedRegions();

      if (!context.RegionTaxes.Any())
        SeedRegionTaxes();
    }

    private static void SeedCalculationTypes()
    {
      _context.CalculationTypes.AddRange(
        new List<CalculationType>()
        {
          new CalculationType
          {
            Name = "Progressive",
            Status = RecordStatus.Active
          },
          new CalculationType
          {
            Name = "Flat Value",
            Status = RecordStatus.Active
          },
          new CalculationType
          {
            Name = "Flat Rate",
            Status = RecordStatus.Active
          }

        });

      _context.SaveChanges();
      _logger.Information("Created default calculation types [Progressive, Flat Value, Flat Rate].");
    }

    private static void SeedRegions()
    {
      _context.Regions.AddRange(
       new List<Region>()
       {
          new Region
          {
            Name = "Cape Town",
            PostalCode = "7441",
            CalculationTypeId = _context.CalculationTypes.First(ct => ct.Name == "Progressive").Id,
            Status = RecordStatus.Active
          },
          new Region
          {
            Name = "Durban",
            PostalCode = "A100",
            CalculationTypeId = _context.CalculationTypes.First(ct => ct.Name == "Flat Value").Id,
            Status = RecordStatus.Active
          },
          new Region
          {
            Name = "Northern Cape",
            PostalCode = "7000",
            CalculationTypeId = _context.CalculationTypes.First(ct => ct.Name == "Flat Rate").Id,
            Status = RecordStatus.Active
          },
          new Region
          {
            Name = "Gauteng",
            PostalCode = "1000",
            CalculationTypeId = _context.CalculationTypes.First(ct => ct.Name == "Progressive").Id,
            Status = RecordStatus.Active
          }

       });

      _context.SaveChanges();
      _logger.Information("Created default region types.");
    }

    private static void SeedRegionTaxes()
    {
      _context.RegionTaxes.AddRange(
      new List<RegionTax>()
      {
          new RegionTax
          {
            RegionId = _context.Regions.First(ct => ct.Name == "Cape Town").Id,
            FromAmount = 0,
            ToAmount = 8350,
            Rate = 10,
            RateType = RateType.Percentage,
            Status = RecordStatus.Active
          },
          new RegionTax
          {
            RegionId = _context.Regions.First(ct => ct.Name == "Cape Town").Id,
            FromAmount = 8351,
            ToAmount = 33950,
            Rate = 15,
            RateType = RateType.Percentage,
            Status = RecordStatus.Active
          },
          new RegionTax
          {
            RegionId = _context.Regions.First(ct => ct.Name == "Cape Town").Id,
            FromAmount = 33951,
            ToAmount = 82250,
            Rate = 25,
            RateType = RateType.Percentage,
            Status = RecordStatus.Active
          },
          new RegionTax
          {
            RegionId = _context.Regions.First(ct => ct.Name == "Cape Town").Id,
            FromAmount = 82251,
            ToAmount = 171550,
            Rate = 28,
            RateType = RateType.Percentage,
            Status = RecordStatus.Active
          },
          new RegionTax
          {
            RegionId = _context.Regions.First(ct => ct.Name == "Cape Town").Id,
            FromAmount = 171551,
            ToAmount = 372950,
            Rate = 33,
            RateType = RateType.Percentage,
            Status = RecordStatus.Active
          },
          new RegionTax
          {
            RegionId = _context.Regions.First(ct => ct.Name == "Cape Town").Id,
            FromAmount = 372951,
            ToAmount = null,
            Rate = 35,
            RateType = RateType.Percentage,
            Status = RecordStatus.Active
          }
      });
      _context.SaveChanges();
      _logger.Information("Created default region taxes for [Cape Town].");

      _context.RegionTaxes.AddRange(
        new List<RegionTax>()
        {
            new RegionTax
            {
              RegionId = _context.Regions.First(ct => ct.Name == "Durban").Id,
              FromAmount = 0,
              ToAmount = 200000,
              Rate = 5,
              RateType = RateType.Percentage,
            Status = RecordStatus.Active
            },
            new RegionTax
            {
              RegionId = _context.Regions.First(ct => ct.Name == "Durban").Id,
              FromAmount = 200000,
              ToAmount = null,
              Rate = 10000,
              RateType = RateType.Value,
            Status = RecordStatus.Active
            }
        });
      _context.SaveChanges();
      _logger.Information("Created default region taxes for [Durban].");

      _context.RegionTaxes.AddRange(
        new List<RegionTax>()
        {
            new RegionTax
            {
              RegionId = _context.Regions.First(ct => ct.Name == "Northern Cape").Id,
              FromAmount = 0,
              ToAmount = null,
              Rate = 17,
              RateType = RateType.Percentage,
            Status = RecordStatus.Active
            }
        });
      _context.SaveChanges();
      _logger.Information("Created default region taxes for [Northern Cape].");

      _context.RegionTaxes.AddRange(
      new List<RegionTax>()
      {
          new RegionTax
          {
            RegionId = _context.Regions.First(ct => ct.Name == "Gauteng").Id,
            FromAmount = 0,
            ToAmount = 8350,
            Rate = 10,
            RateType = RateType.Percentage,
            Status = RecordStatus.Active
          },
          new RegionTax
          {
            RegionId = _context.Regions.First(ct => ct.Name == "Gauteng").Id,
            FromAmount = 8351,
            ToAmount = 33950,
            Rate = 15,
            RateType = RateType.Percentage,
            Status = RecordStatus.Active
          },
          new RegionTax
          {
            RegionId = _context.Regions.First(ct => ct.Name == "Gauteng").Id,
            FromAmount = 33951,
            ToAmount = 82250,
            Rate = 25,
            RateType = RateType.Percentage,
            Status = RecordStatus.Active
          },
          new RegionTax
          {
            RegionId = _context.Regions.First(ct => ct.Name == "Gauteng").Id,
            FromAmount = 82251,
            ToAmount = 171550,
            Rate = 28,
            RateType = RateType.Percentage,
            Status = RecordStatus.Active
          },
          new RegionTax
          {
            RegionId = _context.Regions.First(ct => ct.Name == "Gauteng").Id,
            FromAmount = 171551,
            ToAmount = 372950,
            Rate = 33,
            RateType = RateType.Percentage,
            Status = RecordStatus.Active
          },
          new RegionTax
          {
            RegionId = _context.Regions.First(ct => ct.Name == "Gauteng").Id,
            FromAmount = 372951,
            ToAmount = null,
            Rate = 35,
            RateType = RateType.Percentage,
            Status = RecordStatus.Active
          }
      });
      _context.SaveChanges();
      _logger.Information("Created default region taxes for [Gauteng].");

    }

  }
}
