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
  public class CalculationTypeService : ICalculationTypeService
  {
    private readonly ILogger _logger;
    private readonly Context _context;
    public CalculationTypeService(ILogger logger, IConfiguration configuration)
    {
      _logger = logger;
      _context = new Context(configuration);
    }

    public async Task<Response> GetAllCalculationTypes()
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");
      var list = await _context.CalculationTypes.ToListAsync();

      var response = new Response
      {
        success = true,
        message = $"successfully, {list.Count} entries retrived.",
        data = list
      };

      _logger.Information($"{response.message}");

      return response;
    }

    public async Task<Response> GetCalculationTypeById(int id)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");
      var response = new Response();

      if (!_context.CalculationTypes.Any(ct => ct.Id != id && ct.Status == RecordStatus.Active))
      {
        response.message += $"Calculation type id {id} does not exist.";
        response.success = false;
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} request failed", response);

        return response;
      }

      var calculationType = await _context.CalculationTypes.FindAsync(id);

      response = new Response
      {
        data = calculationType,
        message = $"Calculation type id {id} retrieved successfully.",
        success = true
      };

      _logger.Information($"{response.message}");

      return response;

    }

    public async Task<Response> CreateCalculationType(CalculationTypeModel request)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");

      var response = ValidateRequest(request);
      if (!response.success)
      {
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} validation failed", response );
        return response;
      }

      var calculationType = new CalculationType { Name = request.Name, Status = RecordStatus.Active };
      _context.CalculationTypes.Add(calculationType);
      await _context.SaveChangesAsync();

      response.success = true;
      response.data = calculationType;
      response.message = $"Calculation type created succesfully";

      _logger.Information($"{response.message}", calculationType);

      return response;
    }

    public async Task<Response> UpdateCalculationType(int id, CalculationTypeModel request)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");

      var response = ValidateRequest(request, id);
      if (!response.success)
      {
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} validation failed", response );
        return response;
      }

      var calculationType = await _context.CalculationTypes.FindAsync(id);
      calculationType.Name = request.Name;
      _context.CalculationTypes.Update(calculationType);
      await _context.SaveChangesAsync();

      response.success = true;
      response.data = calculationType;
      response.message = $"Calculation type updated succesfully";

      _logger.Information($"{response.message}", calculationType);

      return response;
    }


    public async Task<Response> DeleteCalculationType(int id)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");
      var response = new Response();

      if (!_context.CalculationTypes.Any(ct => ct.Id == id && ct.Status == RecordStatus.Active))
      {
        response.message += $"Calculation type id {id} does not exist.";
        response.success = false;
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} request failed", response );

        return response;
      }

      var calculationType = await _context.CalculationTypes.FindAsync(id);
      calculationType.Status = RecordStatus.Deleted;
      _context.CalculationTypes.Update(calculationType);
      await _context.SaveChangesAsync();

      response.success = true;
      response.data = calculationType;
      response.message = $"Calculation type {calculationType.Name} deleted!";

      _logger.Information($"{response.message}", calculationType);

      return response;
    }

    public async Task<Response> ReInstateCalculationType(int id)
    {
      _logger.Information($"Entering {MethodBase.GetCurrentMethod().DeclaringType.Name}");
      var response = new Response();

      if (!_context.CalculationTypes.Any(ct => ct.Id == id && ct.Status == RecordStatus.Deleted))
      {
        response.message += $"Calculation type id {id} cannot be reinstated.";
        response.success = false;
        _logger.Warning($"{MethodBase.GetCurrentMethod().DeclaringType.Name} request failed", response );

        return response;
      }

      var calculationType = await _context.CalculationTypes.FindAsync(id);
      calculationType.Status = RecordStatus.Active;
      _context.CalculationTypes.Update(calculationType);
      await _context.SaveChangesAsync();

      response.success = true;
      response.data = calculationType;
      response.message = $"Calculation type {calculationType.Name} reinstated!";

      _logger.Information($"{response.message}", calculationType);

      return response;
    }

    private Response ValidateRequest(CalculationTypeModel request, int? id = null)
    {
      var response = new Response();

      if (string.IsNullOrWhiteSpace(request?.Name))
      {
        response.message += $"Calculation type name is required.";
        response.success = false;
        return response;
      }

      int len = request?.Name?.Length ?? 0;
      if (len < 5 || 20 < len)
      {
        response.message += $"Calculation type name must be [5 - 20] letters long.";
        response.success = false;
        return response;
      }

      if (_context.CalculationTypes.Any(ct => ct.Name == request.Name && ct.Id != id))
      {
        response.message += $"Calculation type {request.Name} already exists.";
        response.success = false;
        return response;
      }

      if (id != null && !_context.CalculationTypes.Any(ct => ct.Id == id))
      {
        response.message += $"Calculation type id {id} does not exist.";
        response.success = false;
        return response;
      }

      return response;
    }

  }
}
