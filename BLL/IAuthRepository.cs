using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IAuthRepository
    {
        User Register(User user, string password);
        Task<Object> LoginAsync(string username, string password);
      
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

        Task<bool> UserExists(string username);

        Task<User> Register(User user);
    }
}
