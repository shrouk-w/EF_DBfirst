using EF_DBFIRST.DAL;
using EF_DBFIRST.Models.DTOs;
using EF_DBFIRST.Services;
using Microsoft.AspNetCore.Mvc;

namespace EF_DBFIRST.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TripsController : ControllerBase
{
    
    private readonly ITripsService _tripsService;

    public TripsController(ITripsService tripsService)
    {
        _tripsService = tripsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTrips(CancellationToken token,int page = 1, int pageSize = 10)
    {
        var response = await _tripsService.GetAllTrips(page, pageSize, token);
        return Ok(response);
    }

    [HttpPost("{id}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int id, [FromBody] AssignClientToTripRequestDTO dto,
        CancellationToken token)
    {
        await _tripsService.AssignClientToTrip(id, dto, token);
        return Ok();
    }
    
}