
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Restaurants.Domain.Entities;

public class User : IdentityUser
{
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationality { get; set; }

    public List<Restaurant> OwnedRestaurants { get; set; } = new List<Restaurant>();
}
