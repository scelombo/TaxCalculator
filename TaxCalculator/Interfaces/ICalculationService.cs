using System.Threading.Tasks;
using TaxCalculator.DTO;
using TaxCalculator.Utilities;

namespace TaxCalculator.Interfaces
{
  public interface ICalculationService
  {
    Task<Response> ProcessCalculation(CalculationModel request);
  }
}
