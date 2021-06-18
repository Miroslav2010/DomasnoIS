using Domain.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<CustomUser> entities;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<CustomUser>();
        }
        public IEnumerable<CustomUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public CustomUser Get(string id)
        {
            return entities
               .Include(z => z.UserCart)
               .Include("UserCart.Tickets")
               .Include("UserCart.Tickets.Ticket")
               .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(CustomUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(CustomUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(CustomUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
