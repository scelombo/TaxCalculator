using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaxCalculator.DTO;
using TaxCalculator.Interfaces;
using TaxCalculator.Models;

namespace TaxCalculator.Pages.Regions
{
  public class CreateModel : PageModel
  {
    private readonly IRegionService _regionService;
    private readonly ICalculationTypeService _calculationTypeService;

    public CreateModel(IRegionService regionService, ICalculationTypeService calculationTypeService)
    {
      _regionService = regionService;
      _calculationTypeService = calculationTypeService;
    }

    [BindProperty]
    public RegionModel region { get; set; }
    public IEnumerable<CalculationType> calculationTypes { get; set; }
    public async Task OnGet()
    {
      var resp = await _calculationTypeService.GetAllCalculationTypes();
      if (resp.success)
      {
        calculationTypes = resp.data as List<CalculationType>;
      }
    }

    public async Task<IActionResult> OnPost()
    {
      if(ModelState.IsValid)
      {
        var result = await _regionService.CreateRegion(region);
        if (result.success)
          return RedirectToPage("Regions");

        return Page();
      }
      else
      {
        return Page();
      }
    }
  }
}