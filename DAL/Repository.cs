using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Repository<TDbContext> : IRepository where TDbContext : DataContext
    {
        private DataContext _Context;
        public DataContext Context
        {
            get => _Context;
        }
        public Repository(TDbContext context)
        {
            _Context = context;
        }

        public async Task<bool> UserExists(string username) 
        {
            return await _Context.Users.AnyAsync(x => x.UserName == username);
        }

        public async Task<User> GetAsync(string username)
        {

            return await _Context.Set<User>().FirstOrDefaultAsync(o => o.UserName == username);
        }

        public async Task<User> AddUser(User user)
        {

            await _Context.Users.AddAsync(user);
            await _Context.SaveChangesAsync();
            return user;
        }
    }
}
