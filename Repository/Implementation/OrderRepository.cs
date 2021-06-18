using Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {

        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }


        public List<Order> getAllOrders()
        {
            return entities
                .Include(z => z.User)
                .Include(z => z.Tickets)
                .Include("Tickets.Ticket")
                .ToListAsync().Result;
        }

        public Order getOrderDetails(BaseEntity model)
        {
            return entities
               .Include(z => z.User)
               .Include(z => z.Tickets)
               .Include("Tickets.Ticket")
               .SingleOrDefaultAsync(z => z.Id == model.Id).Result;
        }
    }
}
