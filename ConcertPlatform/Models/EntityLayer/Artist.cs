using System;
using System.Collections.Generic;

namespace ConcertPlatform.Models.EntityLayer;

public partial class Artist
{
    public int Artist_Id { get; set; }

    public string Artist_Name { get; set; } = null!;

    public string? Genre { get; set; }

    public string? Bio { get; set; }

    public string? Contact_Information { get; set; }

    public override string ToString()
    {
        return $"Name: {Artist_Name}\n" +
               $"Genre: {Genre}\n" +
               $"Bio: {Bio}\n" +
               $"Contact: {Contact_Information}";
    }
}
