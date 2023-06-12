using System;
using System.Collections.Generic;

namespace ConcertPlatform.Models.EntityLayer;

public partial class Attendee
{
    public int AttendeeId { get; set; }

    public int? TicketId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
