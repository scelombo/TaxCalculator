using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TaxCalculator.DTO;
using TaxCalculator.Interfaces;
using TaxCalculator.Models;
using TaxCalculator.Utilities;

namespace TaxCalculator.Services
{
  public class RegionTaxService : IRegionTaxService
  {
    private readonly ILogger _logger;
    private readonly Context _context;
    public RegionTaxService(ILogger logger, IConfiguration configuration)
    {
      _logger = logger;
      _context = new Context(configuration);
    }

    public async Task<Response> GetAllRegionTaxes(int id)
    {

      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");
      var list = await _context.RegionTaxes.Where(r => r.RegionId == id && r.Status == RecordStatus.Active).ToListAsync();

      var rsp = new List<object>();

      list.ForEach(lst =>
      {
        rsp.Add(
          new
          {
            Id = lst.Id,
            Rate = lst.Rate,
            RegionId = lst.RegionId,
            RateType = lst.RateType.ToString(),
            Status = lst.Status.ToString(),
            FromAmount = lst.FromAmount,
            ToAmount =  lst.ToAmount?.ToString() ?? "Infinite"
          });
      });

      var response = new Response
      {
        success = true,
        message = $"successfully, {list.Count} entries retrived.",
        data = rsp
      };

      _logger.Information($"{response.message}");

      return response;
    }

    public async Task<Response> GetRegionTaxById(int id)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");
      var response = new Response();

      if (!_context.RegionTaxes.Any(rg => rg.Id != id && rg.Status == RecordStatus.Active))
      {
        response.message += $"Region tax id {id} does not exist.";
        response.success = false;
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} request failed", response);

        return response;
      }

      var RegionTax = await _context.RegionTaxes.FindAsync(id);

      response = new Response
      {
        data = RegionTax,
        message = $"Region tax id {id} retrieved successfully.",
        success = true
      };

      _logger.Information($"{response.message}");

      return response;

    }

    public async Task<Response> CreateRegionTax(RegionTaxModel request)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");

      var response = ValidateRequest(request);
      if (!response.success)
      {
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} validation failed", response);
        return response;
      }


      ReshufleTaxLines(request);
      

      var RegionTax = new RegionTax
      {
        FromAmount = request.FromAmount,
        Rate = request.Rate,
        RateType = request.RateType,
        RegionId = request.RegionId,
        ToAmount = request.ToAmount,
        Status = RecordStatus.Active
      };


      _context.RegionTaxes.Add(RegionTax);
      await _context.SaveChangesAsync();

      response.success = true;
      response.data = RegionTax;
      response.message = $"Region tax created succesfully";

      _logger.Information($"{response.message}", RegionTax);

      return response;
    }

    private void ReshufleTaxLines(RegionTaxModel request)
    {
      // The biggest tax amount must always be infinity (null)
      var toAmount = _context.RegionTaxes.Any(t => t.RegionId == request.RegionId && t.ToAmount > request.ToAmount && t.Status == RecordStatus.Active) ? request.ToAmount : null;

      //End the previous line on the current
      if (_context.RegionTaxes.Any(t => t.RegionId == request.RegionId &&
                                   t.FromAmount <= request.ToAmount &&
                                   t.ToAmount == null &&
                                   t.Status == RecordStatus.Active))
      {
        var prevousLine = _context.RegionTaxes.First(t => t.RegionId == request.RegionId &&
                                  t.FromAmount <= request.ToAmount &&
                                  t.ToAmount == null &&
                                  t.Status == RecordStatus.Active);

        prevousLine.ToAmount = request.FromAmount - 0.1;
        _context.RegionTaxes.Update(prevousLine);
        _context.SaveChanges();

        request.ToAmount = toAmount;

      }
    }

    public async Task<Response> UpdateRegionTax(int id, RegionTaxModel request)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");

      var response = ValidateRequest(request, id);
      if (!response.success)
      {
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} validation failed", response);
        return response;
      }

      var RegionTax = await _context.RegionTaxes.FindAsync(id);


      _context.RegionTaxes.Update(RegionTax);
      await _context.SaveChangesAsync();

      response.success = true;
      response.data = RegionTax;
      response.message = $"RegionTax updated succesfully";

      _logger.Information($"{response.message}", RegionTax);

      return response;
    }


    public async Task<Response> DeleteRegionTax(int id)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");
      var response = new Response();

      if (!_context.RegionTaxes.Any(rg => rg.Id == id && rg.Status == RecordStatus.Active))
      {
        response.message += $"RegionTax id {id} does not exist.";
        response.success = false;
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} request failed", response);

        return response;
      }

      var RegionTax = await _context.RegionTaxes.FindAsync(id);
      RegionTax.Status = RecordStatus.Deleted;
      _context.RegionTaxes.Update(RegionTax);
      await _context.SaveChangesAsync();

      response.success = true;
      response.data = RegionTax;
      response.message = $"RegionTax deleted!";

      _logger.Information($"{response.message}", RegionTax);

      return response;
    }

    public async Task<Response> ReInstateRegionTax(int id)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");
      var response = new Response();

      if (!_context.RegionTaxes.Any(rg => rg.Id == id && rg.Status == RecordStatus.Deleted))
      {
        response.message += $"RegionTax id {id} cannot be reinstated.";
        response.success = false;
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} request failed", response);

        return response;
      }

      var RegionTax = await _context.RegionTaxes.FindAsync(id);
      RegionTax.Status = RecordStatus.Active;
      _context.RegionTaxes.Update(RegionTax);
      await _context.SaveChangesAsync();

      response.success = true;
      response.data = RegionTax;
      response.message = $"Region tax reinstated!";

      _logger.Information($"{response.message}", RegionTax);

      return response;
    }

    private Response ValidateRequest(RegionTaxModel request, int? id = null)
    {
      var response = new Response();

      if (request.FromAmount < 0)
      {
        response.message += $"Form amount cannoot be less then 0.";
        response.success = false;
        return response;
      }

      if (request.ToAmount != null && request.ToAmount < 0)
      {
        response.message += $"To amount cannoot be less then 0.";
        response.success = false;
        return response;
      }

      if (request?.ToAmount != null && request.ToAmount <= request.FromAmount)
      {
        response.message += $"To cannoot be less or equal to the from amount";
        response.success = false;
        return response;
      }

      //if (_context.RegionTaxes.Any( rt => rt.ToAmount >= request.ToAmount  || rt.FromAmount <= request.FromAmount))
      //{
      //  response.message += $"Region Tax amount should not overlap. Only one rate must exist per rage.";
      //  response.success = false;
      //  return response;
      //}

      if (request.Rate <= 0)
      {
        response.message += $"Tax rate must always be greater than 0";
        response.success = false;
        return response;
      }

      //if (_context.RegionTaxes.Any(rt => rt.ToAmount >= request.ToAmount && (rt.RateType == request.RateType && rt.Rate >= request.Rate)))
      //{
      //  response.message += $"Tax rate of {request.Rate} is already set for an equal or less tax bracket.";
      //  response.success = false;
      //  return response;
      //}

      return response;
    }
  }
}
