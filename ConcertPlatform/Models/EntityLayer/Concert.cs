﻿using System;
using System.Collections.Generic;

namespace ConcertPlatform.Models.EntityLayer;

public partial class Concert
{
    public int ConcertId { get; set; }

    public int? ArtistId { get; set; }

    public int? VenueId { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public decimal? TicketPrice { get; set; }

    public string? Description { get; set; }
}
