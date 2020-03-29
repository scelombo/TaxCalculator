using System.ComponentModel.DataAnnotations;
using TaxCalculator.Utilities;

namespace TaxCalculator.DTO
{
  public class RegionTaxModel
  {
    public double FromAmount { get; set; }
    public double? ToAmount { get; set; }
    [Required]
    public RateType RateType { get; set; }
    [Required]
    public double Rate { get; set; }
    public int RegionId { get; internal set; }
  }
}
