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
  public class RegionsController : ControllerBase
  {
    private readonly ILogger _logger;
    private readonly IRegionService _regionService;

    public RegionsController(IRegionService regionService, ILogger logger)
    {
      _logger = logger;
      _regionService = regionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRegions()
    {
      try
      {
        var rsp = await _regionService.GetAllRegions();
        return Ok(rsp);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error occured trying to retrive regions.");
        return BadRequest("Error occured trying to retrive regions");
      }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllRegionById(int id)
    {
      try
      {
        var rsp = await _regionService.GetRegionById(id);
        return Ok(rsp);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error occured trying to retrive region.");
        return BadRequest("Error occured trying to retrive region");
      }
    }

    [HttpPost]
    public async Task<IActionResult> CreateRegion(RegionModel request)
    {
      try
      {
        var rsp = await _regionService.CreateRegion(request);
        return Ok(rsp);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error occured trying to insert region.");
        return BadRequest("Error occured trying to insert region");
      }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRegion(int id, RegionModel request)
    {
      try
      {
        var rsp = await _regionService.UpdateRegion(id, request);
        return Ok(rsp);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error occured trying to update region.");
        return BadRequest("Error occured trying to update region");
      }
    }

       
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        var rsp = await _regionService.DeleteRegion(id);
        return Ok(rsp);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error occured trying to detele region.");
        return BadRequest("Error occured trying to detele region");
      }
    }

    [HttpPut("{id}/Reinstate")]
    public async Task<IActionResult> Reinstate(int id)
    {
      try
      {
        var rsp = await _regionService.ReInstateRegion(id);
        return Ok(rsp);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error occured trying to reInstate region.");
        return BadRequest("Error occured trying to reInstate region");
      }
    }

  }
}