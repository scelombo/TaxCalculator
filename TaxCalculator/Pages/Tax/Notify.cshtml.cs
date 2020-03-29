using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace TaxCalculator.Pages.Tax
{
  public class NotifyModel : PageModel
  {
    public string msg;
    public bool successful;
    public int RegionId;
    public void OnGet(bool success, string message, int regionId)
    {
      msg = message;
      successful = success;
      RegionId = regionId;
    }

    public IActionResult OnPost()
    {
      return RedirectToPage("TaxRegions", new
      {
        Id = int.Parse(Request.Query["regionId"])
      });
    }
  }
}