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
    public async Task<IActionResult> getAllTrips(CancellationToken token,int page = 1, int pageSize = 10)
    {
        var response = await _tripsService.GetAllTrips(page, pageSize, token);
        return Ok(response);
    }
}