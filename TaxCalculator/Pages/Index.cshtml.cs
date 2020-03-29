using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TaxCalculator.DTO;
using TaxCalculator.Interfaces;

namespace TaxCalculator.Pages
{
  public class IndexModel : PageModel
  {
    private readonly ICalculationService _calculationService;
    public IndexModel(ICalculationService calculationService)
    {
      _calculationService = calculationService;
    }

    [BindProperty]
    public CalculationModel calculationModel { get; set; }
    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPost()
    {
      var result = await _calculationService.ProcessCalculation(calculationModel);
      return RedirectToPage("Notify", new
      {
        success = result.success,
        message = result.message
      });

    }
  }
}
