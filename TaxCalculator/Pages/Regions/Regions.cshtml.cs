using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaxCalculator.Interfaces;
using TaxCalculator.Models;

namespace TaxCalculator.Pages
{
  public class RegionsModel : PageModel
  {
    private readonly IRegionService _regionService;
    
    public RegionsModel(IRegionService regionService)
    {
      _regionService = regionService;
    }

    public async Task<IActionResult> OnPostDelete(int id)
    {
       var result = await _regionService.GetRegionById(id);
      if (result.success)
      {
        var delete = await _regionService.DeleteRegion(id);
        if (delete.success)
          return Redirect("Regions");
        return BadRequest(delete.message);
      }

      return BadRequest(result.message);
    }
  }
}