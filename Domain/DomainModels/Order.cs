using Domain.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public CustomUser User { get; set; }
        public virtual ICollection<TicketInOrder> Tickets { get; set; }
    }
}
