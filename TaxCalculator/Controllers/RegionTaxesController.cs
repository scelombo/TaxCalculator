using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;
using TaxCalculator.DTO;
using TaxCalculator.Interfaces;

namespace TaxCalculator.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RegionTaxesController : ControllerBase
  {
    private readonly ILogger _logger;
    private readonly IRegionTaxService _regionTaxes;

    public RegionTaxesController(IRegionTaxService regionTaxes, ILogger logger)
    {
      _logger = logger;
      _regionTaxes = regionTaxes;
    }

    [HttpGet("{id}/list")]
    public async Task<IActionResult> GetAllRegionTaxes(int id)
    {
      try
      {
        var rsp = await _regionTaxes.GetAllRegionTaxes(id);
        return Ok(rsp);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error occured trying to retrive calculation type table.");
        return BadRequest("Error occured trying to retrive calculation type table.");
      }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRegionTaxById(int id)
    {
      try
      {
        var rsp = await _regionTaxes.GetRegionTaxById(id);
        return Ok(rsp);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error occured trying to retrive calculation type table.");
        return BadRequest("Error occured trying to retrive calculation type table.");
      }
    }

    [HttpPost]
    public async Task<IActionResult> CreateRegionTax(RegionTaxModel request)
    {
      try
      {
        var rsp = await _regionTaxes.CreateRegionTax(request);
        return Ok(rsp);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error occured trying to insert calculation type table.");
        return BadRequest("Error occured trying to insert calculation type table.");
      }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRegionTax(int id, RegionTaxModel request)
    {
      try
      {
        var rsp = await _regionTaxes.UpdateRegionTax(id, request);
        return Ok(rsp);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error occured trying to update calculation type table.");
        return BadRequest("Error occured trying to update calculation type table.");
      }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        var rsp = await _regionTaxes.DeleteRegionTax(id);
        return Ok(rsp);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error occured trying to detele calculation type table.");
        return BadRequest("Error occured trying to detele calculation type table");
      }
    }

    [HttpPut("{id}/Reinstate")]
    public async Task<IActionResult> Reinstate(int id)
    {
      try
      {
        var rsp = await _regionTaxes.ReInstateRegionTax(id);
        return Ok(rsp);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error occured trying to reInstate calculation type table.");
        return BadRequest("Error occured trying to reInstate calculation type table.");
      }
    }
  }
}
