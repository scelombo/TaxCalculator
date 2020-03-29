using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaxCalculator.Pages.Tax
{
  public class NotifyModel : PageModel
  {
    public string msg;
    public bool successful;
    public void OnGet(bool success, string message, int? regionId)
    {
      msg = message;
      successful = success;
    }

    public IActionResult OnPost()
    {
      if (int.TryParse(Request.Query["regionId"], out int regionId))
        return RedirectToPage("/Tax/TaxRegions", new
        {
          Id = regionId
        });

      return RedirectToPage("Index");
    }
  }
}