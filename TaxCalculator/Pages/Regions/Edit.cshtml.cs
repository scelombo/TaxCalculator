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
    public class EditModel : PageModel
    {
    private readonly IRegionService _regionService;
    private readonly ICalculationTypeService _calculationTypeService;

    public EditModel(IRegionService regionService, ICalculationTypeService calculationTypeService)
    {
      _regionService = regionService;
      _calculationTypeService = calculationTypeService;
    }

    [BindProperty]
    public RegionModel region { get; set; }
    public IEnumerable<CalculationType> calculationTypes { get; set; }
    public async Task OnGet(int id)
    {

      var resp = await _calculationTypeService.GetAllCalculationTypes();
      if (resp.success)
      {
        calculationTypes = resp.data as List<CalculationType>;
      }

      var result = await _regionService.GetRegionById(id);
      if (resp.success)
      {
        var model = result.data as Region;
        region = new RegionModel
        {
          CalculationTypeId = model.CalculationTypeId,
          Name = model.Name,
          PostalCode = model.PostalCode
        };
      }

    }

    public async Task<IActionResult> OnPost(int id)
    {
      if (ModelState.IsValid)
      {
        var result = await _regionService.UpdateRegion(id, region);
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