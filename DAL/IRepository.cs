using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository
    {
        Task<bool> UserExists(string username);
        Task<User> GetAsync(string username);

        Task<User> AddUser(User user);

    }
}