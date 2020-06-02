using System.Threading.Tasks;
using CloudCityCakeCo.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudCityCakeCo.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<User> AddUserAsync(User user)
        {
            var entity = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context
                .Users
                .FirstOrDefaultAsync(e => e.Number == phoneNumber);
        }
    }
}