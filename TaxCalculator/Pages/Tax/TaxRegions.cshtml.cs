using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaxCalculator.DTO;
using TaxCalculator.Interfaces;
using TaxCalculator.Models;
using TaxCalculator.Utilities;

namespace TaxCalculator.Pages.Tax
{
  public class TaxRegionsModel : PageModel
  {
    private readonly IRegionService _regionService;
    private readonly IRegionTaxService _regionTaxService;
    public TaxRegionsModel(IRegionService regionService, IRegionTaxService regionTaxService)
    {
      _regionService = regionService;
      _regionTaxService = regionTaxService;
    }

    public Region region { get; set; }

    [BindProperty]
    public RegionTaxModel regionTax { get; set; }
    public IEnumerable<CalculationType> calculationTypes { get; set; }

    public int RegionId;
    public async Task OnGet(int id)
    {
      RegionId = id;
      regionTax = new RegionTaxModel
      {
        RegionId = id
      };

      var result = await _regionService.GetRegionById(id);
      if (result.success)
      {
        region = result.data as Region;
      }
    }

    public async Task<IActionResult> OnPost()
    {
      regionTax.RegionId =  int.Parse(Request.Query["Id"]);
      var result = await _regionTaxService.CreateRegionTax(regionTax);
      if (result.success)
      {
        region = result.data as Region;
      }

      return RedirectToPage("Notify", new
      {
        success = result.success,
        message = result.message,
        regionId = regionTax.RegionId
      }
      );

    }
  }
}