using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcertPlatform.Models.EntityLayer
{
    public class ShoppingBasket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TicketId { get; set; }
    }
}