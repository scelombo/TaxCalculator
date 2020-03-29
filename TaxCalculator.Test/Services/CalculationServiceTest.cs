using NUnit.Framework;
using TaxCalculator.Interfaces;
using TaxCalculator.Services;

namespace TaxCalculator.Test
{
  public class CalculationServiceTest
  {
    private ICalculationService _calculationService;

    [SetUp]
    public void Setup()
    {
      //_calculationService = new CalculationService();
    }

    [Test]
    public void When_NullValuesSubmited_shouldFail()
    {
      //Arrange

      //Process

      //Asset
      Assert.Pass();
    }

    [Test]
    public void When_NoPostalCodeSubmited_shouldFail()
    {
      //Arrange

      //Process

      //Asset
      Assert.Pass();
    }

    [Test]
    public void When_UnknownPostalCodeSubmited_shouldFail()
    {
      //Arrange

      //Process

      //Asset
      Assert.Pass();
    }

    [Test]
    public void When_NegativeIncomeSubmited_shouldFail()
    {
      //Arrange

      //Process

      //Asset
      Assert.Pass();
    }

    [Test]
    public void When_ZeroIncomeSubmited_shouldReatunZeroTax()
    {
      //Arrange

      //Process

      //Asset
      Assert.Pass();
    }

    [Test]
    public void When_CorrectValuesSubmited_ShouldReatunValidTax()
    {
      //Arrange

      //Process

      //Asset
      Assert.Pass();
    }
  }
}
