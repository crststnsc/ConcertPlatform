using System;
using System.Collections.Generic;

namespace ConcertPlatform.Models.EntityLayer;

public partial class User
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public bool Isadmin { get; set; }
}
