namespace TaxCalculator.Utilities
{
  public class Response
  {
    public bool success { get; set; } = true;
    public string message { get; set; } = string.Empty;
    public object data { get; set; }
  }
}
