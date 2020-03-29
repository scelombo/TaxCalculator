using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TaxCalculator.DTO;
using TaxCalculator.Interfaces;
using TaxCalculator.Models;
using TaxCalculator.Utilities;

namespace TaxCalculator.Services
{
  public class RegionService : IRegionService
  {
    private readonly ILogger _logger;
    private readonly Context _context;
    public RegionService(ILogger logger, IConfiguration configuration)
    {
      _logger = logger;
      _context = new Context(configuration);
    }

    public async Task<Response> GetAllRegions()
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");
      var list = await _context.Regions.Where(r => r.Status == RecordStatus.Active).Include(rg => rg.CalculationType).ToListAsync();

      var response = new Response
      {
        success = true,
        message = $"successfully, {list.Count} entries retrived.",
        data = list
      };

      _logger.Information($"{response.message}");

      return response;
    }

    public async Task<Response> GetRegionById(int id)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");
      var response = new Response();

      if (!_context.Regions.Any(rg => rg.Id != id && rg.Status == RecordStatus.Active))
      {
        response.message += $"Region id {id} does not exist.";
        response.success = false;
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} request failed", response);

        return response;
      }

      var region = await _context.Regions.FindAsync(id);

      response = new Response
      {
        data = region,
        message = $"Region id {id} retrieved successfully.",
        success = true
      };

      _logger.Information($"{response.message}");

      return response;

    }

    public async Task<Response> CreateRegion(RegionModel request)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");

      var response = ValidateRequest(request);
      if (!response.success)
      {
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} validation failed", response);
        return response;
      }

      var region = new Region
      {
        Name = request.Name,
        PostalCode = request.PostalCode,
        CalculationTypeId = request.CalculationTypeId,
        Status = RecordStatus.Active        
      };


      _context.Regions.Add(region);
      await _context.SaveChangesAsync();

      response.success = true;
      response.data = region;
      response.message = $"Region created succesfully";

      _logger.Information($"{response.message}", region);

      return response;
    }

    public async Task<Response> UpdateRegion(int id, RegionModel request)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");

      var response = ValidateRequest(request, id);
      if (!response.success)
      {
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} validation failed", response);
        return response;
      }

      var region = await _context.Regions.FindAsync(id);
      region.Name = request.Name;
      region.PostalCode = request.PostalCode;
      region.CalculationTypeId = request.CalculationTypeId;

      _context.Regions.Update(region);
      await _context.SaveChangesAsync();

      response.success = true;
      response.data = region;
      response.message = $"Region updated succesfully";

      _logger.Information($"{response.message}", region);

      return response;
    }


    public async Task<Response> DeleteRegion(int id)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");
      var response = new Response();

      if (!_context.Regions.Any(rg => rg.Id == id && rg.Status == RecordStatus.Active))
      {
        response.message += $"Region id {id} does not exist.";
        response.success = false;
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} request failed", response);

        return response;
      }

      var region = await _context.Regions.FindAsync(id);
      region.Status = RecordStatus.Deleted;
      _context.Regions.Update(region);
      await _context.SaveChangesAsync();

      response.success = true;
      response.data = region;
      response.message = $"Region {region.Name} deleted!";

      _logger.Information($"{response.message}", region);

      return response;
    }

    public async Task<Response> ReInstateRegion(int id)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");
      var response = new Response();

      if (!_context.Regions.Any(rg => rg.Id == id && rg.Status == RecordStatus.Deleted))
      {
        response.message += $"Region id {id} cannot be reinstated.";
        response.success = false;
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} request failed", response);

        return response;
      }

      var region = await _context.Regions.FindAsync(id);
      region.Status = RecordStatus.Active;
      _context.Regions.Update(region);
      await _context.SaveChangesAsync();

      response.success = true;
      response.data = region;
      response.message = $"Region {region.Name} reinstated!";

      _logger.Information($"{response.message}", region);

      return response;
    }

    private Response ValidateRequest(RegionModel request, int? id = null)
    {
      var response = new Response();

      if (string.IsNullOrWhiteSpace(request?.Name))
      {
        response.message += $"Region name is required.";
        response.success = false;
        return response;
      }

      int len = request?.Name?.Length ?? 0;
      if (len < 5 || 50 < len)
      {
        response.message += $"Region name must be [5 - 50] letters long.";
        response.success = false;
        return response;
      }

      if (_context.Regions.Any(rg => rg.Name == request.Name && rg.Id != id))
      {
        response.message += $"Region {request.Name} already exists.";
        response.success = false;
        return response;
      }


      if (string.IsNullOrWhiteSpace(request?.PostalCode))
      {
        response.message += $"Postal code is required.";
        response.success = false;
        return response;
      }

      if (!request.PostalCode.All(char.IsDigit))
      {
        response.message += $"Postal code can only be digits.";
        response.success = false;
        return response;
      }

      int pcodeLen = request?.PostalCode?.Length ?? 0;
      if (pcodeLen < 4 || 7 < pcodeLen)
      {
        response.message += $"Region name must be [4 - 7] digits long.";
        response.success = false;
        return response;
      }

      if (_context.Regions.Any(rg => rg.PostalCode == request.PostalCode && rg.Id != id))
      {
        response.message += $"Postal code {request.PostalCode} already exists.";
        response.success = false;
        return response;
      }

      if (id != null && !_context.CalculationTypes.Any(ct => ct.Id == request.CalculationTypeId && ct.Status == RecordStatus.Active))
      {
        response.message += $"Cannot use calclulation id {request.CalculationTypeId} for region {request.Name}. Unknown calclulation type.";
        response.success = false;
        return response;
      }

      if (id != null && !_context.Regions.Any(rg => rg.Id == id))
      {
        response.message += $"Region id {id} does not exist.";
        response.success = false;
        return response;
      }

      return response;
    }
  }
}
