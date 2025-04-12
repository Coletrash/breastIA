using BreastIA.Data;
using BreastIA.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BreastIA.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

      
        public async Task UpdateTokenExpirationAsync(string token)
        {
            var userToken = await _context.UserTokens
                .Where(t => t.Token == token)
                .FirstOrDefaultAsync();

            if (userToken != null)
            {
              
                userToken.Expiration = DateTime.UtcNow.AddMinutes(10);
                _context.UserTokens.Update(userToken);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GenerateTokenAsync(int userId)
        {
            var token = Guid.NewGuid().ToString("N");

            var userToken = new UserToken
            {
                UserId = userId,
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };

            await _context.UserTokens.AddAsync(userToken);
            await _context.SaveChangesAsync();

            return token; 
        }

       
        public async Task<User> ValidateTokenAsync(string token)
        {
            var userToken = await _context.UserTokens
                .Where(t => t.Token == token && t.IsUsed == false && t.Expiration > DateTime.UtcNow)
                .Include(t => t.User)
                .FirstOrDefaultAsync();

            if (userToken == null)
                return null; 

          
            userToken.IsUsed = true;
            _context.UserTokens.Update(userToken);
            await _context.SaveChangesAsync();

            return userToken.User; // Devuelve el usuario asociado al token
        }

        
        public async Task LogoutAsync(string token)
        {
            var userToken = await _context.UserTokens
                .Where(t => t.Token == token)
                .FirstOrDefaultAsync();

            if (userToken != null)
            {
                _context.UserTokens.Remove(userToken);
                await _context.SaveChangesAsync();
            }
        }

    
        public async Task<User> ValidateUserAsync(string email, string password)
        {
       
            var user = await _context.Users
                .Where(u => u.Email == email && u.Password == password)  
                .FirstOrDefaultAsync();

            return user; 
        }
    }
}
