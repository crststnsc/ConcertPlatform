using System;
using System.Collections.Generic;

namespace ConcertPlatform.Models.EntityLayer;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string ArtistName { get; set; } = null!;

    public string? Genre { get; set; }

    public string? Bio { get; set; }

    public string? ContactInformation { get; set; }

    public virtual ICollection<Concert> Concerts { get; set; } = new List<Concert>();
}
