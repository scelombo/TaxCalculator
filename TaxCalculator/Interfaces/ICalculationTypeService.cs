using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.DTO;
using TaxCalculator.Models;
using TaxCalculator.Utilities;

namespace TaxCalculator.Interfaces
{
  public interface ICalculationTypeService
  {
    Task<Response> GetAllCalculationTypes();
    Task<Response> GetCalculationTypeById(int id);
    Task<Response> CreateCalculationType(CalculationTypeModel request);
    Task<Response> UpdateCalculationType(int id, CalculationTypeModel request);
    Task<Response> DeleteCalculationType(int id);
    Task<Response> ReInstateCalculationType(int id);
  }
}
