using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcertPlatform.Models.EntityLayer
{
    public class BasketItem
    {
        public int TicketId { get; set; }
        public string TicketHolderName { get; set; }
        public int SeatNumber { get; set; }
        public int ConcertId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public decimal TicketPrice { get; set; }
        public string ArtistName { get; set; }

        //override to string
        public override string ToString()
        {
            return $"Date: {Date.ToShortDateString()}, Time: {Time}, Artist: {ArtistName}, Price: {TicketPrice}, " +
                $"Ticket holder name: {TicketHolderName}, Seat number: {SeatNumber}";
        }
    }
}
