using System.ComponentModel.DataAnnotations;
using TaxCalculator.Utilities;

namespace TaxCalculator.Models
{
  public class Region
  {
    [Key]
    public int Id { get; set; }
    [Required, MinLength(5), MaxLength(50)]
    public string Name { get; set; }

    [Required, MinLength(4), MaxLength(7)]
    public string PostalCode { get; set; }
    public int CalculationTypeId { get; set; }
    public CalculationType CalculationType { get; set; }
    public RecordStatus Status { get; set; }
  }
}
