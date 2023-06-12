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

    public virtual ICollection<Concert> Concerts { get; set; } = new List<Concert>();
}
