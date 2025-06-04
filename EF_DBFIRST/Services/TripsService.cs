using EF_DBFIRST.DAL;
using EF_DBFIRST.Exceptions;
using EF_DBFIRST.Models;
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

    public async Task AssignClientToTrip(int id, AssignClientToTripRequestDTO dto, CancellationToken token)
    {
        if(id < 1)
            throw new BadRequestException("Id must be greater than 0");
        
        Trip trip = await _context.Trips.FindAsync(id);
        if(trip == null)
            throw new NotFoundException("Trip not found");
        
        if(trip.DateFrom < DateTime.Now)
            throw new BadRequestException("trip date cannot be in the past");
        
        Client client;

        var existingClient = await _context.Clients
            .FirstOrDefaultAsync(c => c.Pesel == dto.Pesel, token);

        if (existingClient == null)
        {
            client = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Pesel = dto.Pesel
            };

            await _context.Clients.AddAsync(client, token);
            await _context.SaveChangesAsync(token);
        }
        else
        {
            client = existingClient;
        }
        
        bool alreadyAssigned = await _context.ClientTrips
            .AnyAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == id, token);

        if (alreadyAssigned)
        {
            throw new ConflictException("Client is already assigned to this trip.");
        }

        ClientTrip ct = new ClientTrip()
        {
            IdClient = client.IdClient,
            IdTrip = id,
            PaymentDate = dto.PaymentDate,
            RegisteredAt = DateTime.Now,
        };
        
        await _context.ClientTrips.AddAsync(ct, token);
        
        await _context.SaveChangesAsync(token);

    }
}