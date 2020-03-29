using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.DTO;
using TaxCalculator.Models;
using TaxCalculator.Utilities;

namespace TaxCalculator.Interfaces
{
  public interface IRegionService
  {
    Task <Response> GetAllRegions();
    Task <Response> GetRegionById(int id);
    Task <Response> CreateRegion(RegionModel request);
    Task <Response> UpdateRegion(int id, RegionModel request);
    Task <Response> DeleteRegion(int id);
    Task <Response> ReInstateRegion(int id);
  }
}
