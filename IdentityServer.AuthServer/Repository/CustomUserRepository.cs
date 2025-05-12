using IdentityServer.AuthServer.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.AuthServer.Repository
{
    public class CustomUserRepository(CustomDbContext _context): ICustomUserRepository
    {
        //private readonly CustomDbContext _context;
        //public CustomUserRepository(CustomDbContext context)
        //{
        //    _context = context;
        //}
        public async Task<bool> VallidateUser(string email, string password)
        {
            return await _context.customUsers.AnyAsync(x => x.Email == email && x.Password == password);
 
        }
        public async Task<CustomUser?> FindById(int id)
        {
            return await _context.customUsers.FindAsync(id);
        }
        public async Task<CustomUser?> FindByEmail(string email)
        {
            return await _context.customUsers.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
