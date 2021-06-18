
using Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class AddToShoppingCartDto
    {
        public Ticket Ticket { get; set; }
        public Guid TicketId { get; set; }
        public int Quantity { get; set; }
    }
}
