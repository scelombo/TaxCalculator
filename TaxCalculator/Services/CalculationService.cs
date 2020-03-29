using System.Collections.Generic;
using System.Threading.Tasks;
using TaxCalculator.DTO;
using TaxCalculator.Interfaces;
using TaxCalculator.Models;

namespace TaxCalculator.Services
{
  public class CalculationService : ICalculationService
  {
    public Task<Calculation> CreateCalculation(CalculationModel request)
    {
      throw new System.NotImplementedException();
    }

    public Task<Calculation> DeleteCalculation(int id)
    {
      throw new System.NotImplementedException();
    }

    public Task<IEnumerable<Calculation>> GetAllCalculations()
    {
      throw new System.NotImplementedException();
    }

    public Task<Calculation> GetCalculationById(int id)
    {
      throw new System.NotImplementedException();
    }

    public Task<Calculation> ReInstateCalculation(int id)
    {
      throw new System.NotImplementedException();
    }

    public Task<Calculation> UpdateCalculation(int id, CalculationModel request)
    {
      throw new System.NotImplementedException();
    }
  }
}
