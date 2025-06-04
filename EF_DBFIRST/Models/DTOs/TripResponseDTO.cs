namespace EF_DBFIRST.Models.DTOs;

public class TripResponseDTO
{

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }

    public int MaxPeople { get; set; }

    public virtual ICollection<ClientResponseDTO> Clients { get; set; } = new List<ClientResponseDTO>();

    public virtual ICollection<CountryResponseDTO> Countries { get; set; } = new List<CountryResponseDTO>();
}