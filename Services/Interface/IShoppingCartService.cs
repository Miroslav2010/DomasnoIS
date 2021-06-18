using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteTicketFromShoppingCart(string userId, Guid id);
        bool orderNow(string userId);
    }
}
