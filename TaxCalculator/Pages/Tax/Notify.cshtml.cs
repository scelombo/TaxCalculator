using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaxCalculator.Pages.Tax
{
  public class NotifyModel : PageModel
  {
    public string msg;
    public bool successful;
    public int RegionId;
    public void OnGet(bool success, string message, int id)
    {
      msg = message;
      successful = success;
      RegionId = id;
    }
  }
}