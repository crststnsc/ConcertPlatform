using System;
using System.Collections.Generic;

namespace ConcertPlatform.Models.EntityLayer;

public partial class Concert
{
    public int Concert_Id { get; set; }

    public int? Artist_Id { get; set; }

    public int? Venue_Id { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public decimal? Ticket_Price { get; set; }

    public string? Description { get; set; }
}
