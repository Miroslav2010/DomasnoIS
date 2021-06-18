using Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class TicketsIndexDto
    {
        public List<Ticket> Tickets { get; set; }
        public DateTime Date { get; set; }
    }
}
