using Domain.DomainModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.IdentityModels
{
    public enum Role
    {
        ADMIN,
        STANDARD
    }
    public class CustomUser : IdentityUser
    {
        public Role Role { get; set; }
        public virtual ShoppingCart UserCart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
