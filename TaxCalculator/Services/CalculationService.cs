using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TaxCalculator.DTO;
using TaxCalculator.Interfaces;
using TaxCalculator.Models;
using TaxCalculator.Utilities;

namespace TaxCalculator.Services
{
  public class CalculationService : ICalculationService
  {

    private readonly ILogger _logger;
    private readonly Context _context;
    public CalculationService(ILogger logger, IConfiguration configuration)
    {
      _logger = logger;
      _context = new Context(configuration);

    }

    public async Task<Response> ProcessCalculation(CalculationModel request)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");

      var response = ValidateRequest(request);
      if (!response.success)
      {
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} validation failed", response);
        return response;
      }

      _logger.Information($"Getting calclulation metadata");
      var region = _context.Regions.First(r => r.PostalCode == request.PostalCode);
      var regionTax = _context.RegionTaxes.Where(rt => rt.RegionId == region.Id && rt.Status == RecordStatus.Active);

      if(!regionTax.Any())
      {
        _logger.Fatal($"Active region {region.Name} has not tax entries configured.");
        return new Response { 
          success = false,
          message = "Exception occured, please report this error to the system admin - code [0008]"
        };
      }

      _logger.Information($"Processing calclulation.");
      var calculation = CalculateTax(request, (CalcType) region.CalculationTypeId, regionTax);

      _logger.Information($"Save calclulation results.");
      _context.Calculations.Add(calculation);
      await _context.SaveChangesAsync();

      return new Response
      {
        data = calculation,
        success = true,
        message = $"Your total tax amount for the year is : {calculation.Result}"
      };      
    }

    private Calculation CalculateTax(CalculationModel request, CalcType calculationType, IQueryable<RegionTax> regionTax)
    {
      return calculationType == CalcType.Progressive ? ProgressiveTax(request, regionTax) :
             calculationType == CalcType.FlatValue ? FlatValueTax(request, regionTax) :
                                FlatRateTax(request, regionTax);
    }

    private Calculation ProgressiveTax(CalculationModel request, IQueryable<RegionTax> regionTax)
    {
      var taxlines = regionTax.Where(t => request.Income >  t.FromAmount);
      double taxAmount = 0;

      taxlines.ToList().ForEach(tl =>
      {
        taxAmount += CalculateSubLine(request.Income, tl);

      });

      return new Calculation
      {
        AnnualIncome = request.Income,
        Result = taxAmount,
        Date = DateTime.Now,
        PostalCode = request.PostalCode,
        CalclulationMetadata = Newtonsoft.Json.JsonConvert.SerializeObject(regionTax)
      };
    }

    private double CalculateSubLine(double income, RegionTax taxbracket)
    {
      double taxablePortion = 0;
      if (taxbracket.ToAmount != null && income >= taxbracket.ToAmount)
        taxablePortion = taxbracket.ToAmount ?? 0 - taxbracket.FromAmount;
      else
        taxablePortion = income - taxbracket.FromAmount;

      return taxbracket.RateType == RateType.Percentage ? taxablePortion * (taxbracket.Rate / 100) :
                                                            taxbracket.Rate;
    }

    private Calculation FlatValueTax(CalculationModel request, IQueryable<RegionTax> regionTax)
    {

      var tax = regionTax.FirstOrDefault(t => request.Income >= t.FromAmount);

      var taxAmount = tax.RateType == RateType.Percentage ? request.Income * (tax.Rate / 100) :
                                                            tax.Rate;
      return new Calculation
      {
        AnnualIncome = request.Income,
        Result = taxAmount,
        Date = DateTime.Now,
        PostalCode = request.PostalCode,
        CalclulationMetadata = Newtonsoft.Json.JsonConvert.SerializeObject(regionTax)
      };

    }


    private Calculation FlatRateTax(CalculationModel request, IQueryable<RegionTax> regionTax)
    {
      var tax = regionTax.First();
      var taxAmount = tax.RateType == RateType.Percentage ? request.Income * (tax.Rate / 100) :
                                                            tax.Rate;

      return new Calculation
      {
        AnnualIncome = request.Income,
        Result = taxAmount,
        Date = DateTime.Now,
        PostalCode = request.PostalCode,
        CalclulationMetadata = Newtonsoft.Json.JsonConvert.SerializeObject(regionTax)
      };
    }

    private Response ValidateRequest(CalculationModel request)
    {
      var response = new Response();

      if (string.IsNullOrWhiteSpace(request?.PostalCode))
      {
        response.message += $"Postal code is required.";
        response.success = false;
        return response;
      }

      if (!_context.Regions.Any(rg => rg.PostalCode == request.PostalCode && rg.Status == RecordStatus.Active))
      {
        response.message += $"Postal code {request.PostalCode} does not exists for any active regions.";
        response.success = false;
        return response;
      }


      if (request.Income < 0)
      {
        response.message += $"Income must be greater than 0.";
        response.success = false;
        return response;
      }

      return response;
    }
  }
}
