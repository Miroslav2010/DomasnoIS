using Domain.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public CustomUser Owner { get; set; }
        public virtual ICollection<TicketInShoppingCart> Tickets { get; set; }
    }
}
