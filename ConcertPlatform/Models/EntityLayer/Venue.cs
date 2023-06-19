using System;
using System.Collections.Generic;

namespace ConcertPlatform.Models.EntityLayer;

public partial class Venue
{
    public int Venue_Id { get; set; }

    public string Venue_Name { get; set; } = null!;

    public string? Location { get; set; }

    public int? Capacity { get; set; }

    public string? Contact_Information { get; set; }

    public override string ToString()
    {
        return $"Name: {Venue_Name}\n" +
               $"Location: {Location}\n" +
               $"Capacity: {Capacity}\n" +
               $"Contact: {Contact_Information}";
    }
}
