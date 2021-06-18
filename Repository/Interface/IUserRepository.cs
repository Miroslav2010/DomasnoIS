using Domain.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<CustomUser> GetAll();
        CustomUser Get(string id);
        void Insert(CustomUser entity);
        void Update(CustomUser entity);
        void Delete(CustomUser entity);
    }
}
