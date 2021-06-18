using Domain.DomainModels;
using Domain.DTO;
using Repository.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepositorty;
        private readonly IRepository<Order> _orderRepositorty;
        private readonly IRepository<TicketInOrder> _ticketInOrderRepositorty;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<EmailMessage> _mailRepository;

        public ShoppingCartService(IRepository<EmailMessage> mailRepository, 
            IRepository<ShoppingCart> shoppingCartRepository, 
            IRepository<TicketInOrder> ticketInOrderRepositorty, 
            IRepository<Order> orderRepositorty, 
            IUserRepository userRepository)
        {
            _shoppingCartRepositorty = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepositorty = orderRepositorty;
            _ticketInOrderRepositorty = ticketInOrderRepositorty;
            _mailRepository = mailRepository;
        }

        public bool deleteTicketFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.Tickets.Where(z => z.TicketId.Equals(id)).FirstOrDefault();

                userShoppingCart.Tickets.Remove(itemToDelete);

                this._shoppingCartRepositorty.Update(userShoppingCart);

                return true;
            }

            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);

            var userShoppingCart = loggedInUser.UserCart;

            var AllTickets = userShoppingCart.Tickets.ToList();

            var allTicketsPrice = AllTickets.Select(z => new
            {
                TicketPrice = z.Ticket.Price,
                Quantity = z.Quantity
            }).ToList();

            var totalPrice = 0.0;


            foreach (var item in allTicketsPrice)
            {
                totalPrice += item.Quantity * item.TicketPrice;
            }


            ShoppingCartDto scDto = new ShoppingCartDto
            {
                Tickets = AllTickets,
                TotalPrice = totalPrice
            };


            return scDto;

        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                EmailMessage mail = new EmailMessage();
                mail.MailTo = loggedInUser.Email;
                mail.Subject = "Successfully created order";
                mail.Status = false;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this._orderRepositorty.Insert(order);

                List<TicketInOrder> ticketInOrders = new List<TicketInOrder>();

                var result = userShoppingCart.Tickets.Select(z => new TicketInOrder
                {
                    Id = Guid.NewGuid(),
                    TicketId = z.Ticket.Id,
                    Ticket = z.Ticket,
                    OrderId = order.Id,
                    Order = order
                }).ToList();

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0.0;

                sb.AppendLine("Your order is completed. The order conains: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i-1];

                    totalPrice += item.Quantity * item.Ticket.Price;

                    sb.AppendLine(i.ToString() + ". " + item.Ticket.Name + " with price of: " + item.Ticket.Price 
                        + " and quantity of: " + item.Quantity);
                }

                sb.AppendLine("Total price: " + totalPrice.ToString());


                mail.Content = sb.ToString();


                ticketInOrders.AddRange(result);

                foreach (var item in ticketInOrders)
                {
                    this._ticketInOrderRepositorty.Insert(item);
                }

                loggedInUser.UserCart.Tickets.Clear();

                this._userRepository.Update(loggedInUser);
                this._mailRepository.Insert(mail);

                return true;
            }
            return false;
        }
    }
}
