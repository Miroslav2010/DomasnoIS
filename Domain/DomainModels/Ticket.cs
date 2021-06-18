using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DomainModels
{
    public enum Genre
    {
        Comedy,
        Drama,
        Thriller,
        Horror,
        Action,
        Animation,
        SciFi,
        Documentary
    }
    public class Ticket : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Genre Genre { get; set; }
        [Required]
        public double Rating { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        [Required]
        public DateTime DateEnd { get; set; }
        [Required]
        public double Price { get; set; }
        public virtual ICollection<TicketInShoppingCart> ShoppingCarts { get; set; }
        public virtual ICollection<TicketInOrder> Orders { get; set; }
    }
}
