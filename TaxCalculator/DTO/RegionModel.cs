using System.ComponentModel.DataAnnotations;
using TaxCalculator.Utilities;

namespace TaxCalculator.DTO
{
  public class RegionModel
  {
    [Required, MinLength(5), MaxLength(50)]
    public string Name { get; set; }

    [Required, MinLength(4), MaxLength(7)]
    public string PostalCode { get; set; }
    public int CalculationTypeId { get; set; }
  }
}
