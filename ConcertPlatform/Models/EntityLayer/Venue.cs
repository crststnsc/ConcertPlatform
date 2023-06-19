using System;
using System.Collections.Generic;

namespace ConcertPlatform.Models.EntityLayer;

public partial class Venue
{
    public int VenueId { get; set; }

    public string VenueName { get; set; } = null!;

    public string? Location { get; set; }

    public int? Capacity { get; set; }

    public string? ContactInformation { get; set; }

    public override string ToString()
    {
        return $"Name: {VenueName}\n" +
               $"Location: {Location}\n" +
               $"Capacity: {Capacity}\n" +
               $"Contact: {ContactInformation}";
    }
}
