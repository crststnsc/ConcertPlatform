using System;
using System.Collections.Generic;

namespace ConcertPlatform.Models.EntityLayer;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int? ConcertId { get; set; }

    public string? SeatNumber { get; set; }

    public string? TicketHolderName { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();

    public virtual Concert? Concert { get; set; }
}
