using System;
using System.ComponentModel.DataAnnotations;
using TaxCalculator.Utilities;

namespace TaxCalculator.DTO
{
  public class CalculationModel
  {
    [Required]
    public string PostalCode { get; set; }

    [Required]
    public Double Income { get; set; }

  }
}
