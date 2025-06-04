using EF_DBFIRST.DAL;
using EF_DBFIRST.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EF_DBFIRST.Services;

public class TripsService : ITripsService
{
    
    private readonly TripsContext _context;
    
    public TripsService(TripsContext context)
    {
        _context = context;
    }
    
    public async Task<object> GetAllTrips(int page, int pageSize, CancellationToken token)
    {
        GetAllTripsResponseDTO response = new GetAllTripsResponseDTO()
        {
            pageNum = page,
            pageSize = pageSize
        };
        
        int all = await _context.Trips.CountAsync(token);
        
        int allPages = (int)Math.Ceiling((double)all / pageSize);
        
        response.allPages = allPages;
        
        response.Trips = await _context.Trips
                                .OrderByDescending(t=>t.DateFrom)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .Select(t=> new TripResponseDTO()
                                {
                                    Name = t.Name,
                                    Description = t.Description,
                                    DateFrom = t.DateFrom,
                                    MaxPeople = t.MaxPeople,
                                    DateTo = t.DateTo,
                                    Clients = t.ClientTrips
                                        .Select(c => new ClientResponseDTO()
                                        {
                                            FirstName = c.IdClientNavigation.FirstName,
                                            LastName = c.IdClientNavigation.LastName
                                        })
                                        .ToList(),
                                    Countries = t.IdCountries
                                        .Select(c => new CountryResponseDTO()
                                        {
                                            Name = c.Name,
                                        })
                                        .ToList(),
                                })
                                .ToListAsync(token);
        
        return response;
    }
}