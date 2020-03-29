using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.Utilities;

namespace TaxCalculator.Models
{
  public class CalculationType
  {
    [Key]
    public int Id { get; set; }
    [Required, MinLength(5), MaxLength(20)]
    public string Name { get; set; }
    public RecordStatus Status { get; set; }
  }
}
