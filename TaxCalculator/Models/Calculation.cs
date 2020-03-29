using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Models
{
  public class Calculation
  {
    [Key]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string PostalCode { get; set; }
    public double AnnualIncome { get; set; }
    public double Result { get; set; }
    public string CalclulationMetadata { get; set; }

  }
}
