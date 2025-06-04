using EF_DBFIRST.Models;
using EF_DBFIRST.Models.DTOs;

namespace EF_DBFIRST.Services;

public interface ITripsService
{
    
    public Task<object> GetAllTrips(int page, int pageSize, CancellationToken token);
    public Task AssignClientToTrip(int id, AssignClientToTripRequestDTO dto, CancellationToken token);
}