using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.DTO;
using TaxCalculator.Models;

namespace TaxCalculator.Interfaces
{
  public interface ICalculationService
  {
    Task<IEnumerable<Calculation>> GetAllCalculations();
    Task<Calculation> GetCalculationById(int id);
    Task<Calculation> CreateCalculation(CalculationModel request);
    Task<Calculation> UpdateCalculation(int id, CalculationModel request);
    Task<Calculation> DeleteCalculation(int id);
    Task<Calculation> ReInstateCalculation(int id);
  }
}
