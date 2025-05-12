using IdentityServer.AuthServer.Models;

namespace IdentityServer.AuthServer.Repository
{
    public interface ICustomUserRepository
    {
        Task<bool> VallidateUser(string email, string password);
        Task<CustomUser> FindById(int id);
        Task<CustomUser> FindByEmail(string email);
    }
}
