using System.ComponentModel.DataAnnotations;
using TaxCalculator.Utilities;

namespace TaxCalculator.Models
{
  public class RegionTax
  {
    [Key]
    public int Id { get; set; }
    public double FromAmount { get; set; }
    public double? ToAmount { get; set; }
    public RateType RateType { get; set; }
    public double Rate { get; set; }
    public RecordStatus Status { get; internal set; }
    public int RegionId { get; internal set; }
  }
}
