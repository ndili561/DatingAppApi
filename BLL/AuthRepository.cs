using System;
using System.Threading.Tasks;
using DAL.Model;
using DAL;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace BLL
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IRepository _repository;
        
        private readonly IConfiguration _config;

        public AuthRepository(IRepository rep, IConfiguration config)
        {
            _repository = rep;
            
            _config = config;
        }

        public async Task<Object> LoginAsync(string username, string password)
        {
            var user = await _repository.GetAsync(username);
            if (user == null)
            {
                return null;
            }
            else if (this.VerifyPasswordHash(password, user.PassWordHash, user.PassWordSalt))
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return new
                {
                    token = tokenHandler.WriteToken(token)

                };

            }
            return null;
        }

        public async Task<User> Register(User user)
        {
            await _repository.AddUser(this.Register(user,user.Password));
            return user;
        }
    

        public bool VerifyPasswordHash(string password,byte[] passwordHash,byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

            }
            return true;

        }

        public User Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PassWordHash = passwordHash;
            user.PassWordSalt = passwordSalt;
            return user;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordSalt = hmac.Key;

            }
           
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _repository.UserExists(username))
            {
                return true;
            }
            return false;
           
        }

    }
}
