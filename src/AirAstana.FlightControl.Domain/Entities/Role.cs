using System.Collections.Generic;

namespace AirAstana.FlightControl.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    
    public ICollection<User> Users { get; set; } = new List<User>();
    
}