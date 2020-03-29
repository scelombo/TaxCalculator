using System.Threading.Tasks;
using TaxCalculator.DTO;
using TaxCalculator.Utilities;
using System.Collections.Generic;

namespace TaxCalculator.Interfaces

{
  public interface IRegionTaxService
  {
    Task<Response> GetAllRegionTaxes(int id);
    Task<Response> GetRegionTaxById(int id);
    Task<Response> CreateRegionTax(RegionTaxModel request);
    Task<Response> UpdateRegionTax(int id, RegionTaxModel request);
    Task<Response> DeleteRegionTax(int id);
    Task<Response> ReInstateRegionTax(int id);
  }
}
