using Microsoft.EntityFrameworkCore;

namespace EF_DBFIRST.DAL;

public class TripsDbContext : DbContext
{
    
    protected TripsDbContext()
    {
    }

    public TripsDbContext(DbContextOptions options) : base(options)
    {
    }
}