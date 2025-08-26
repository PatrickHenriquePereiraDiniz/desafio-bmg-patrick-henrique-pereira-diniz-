using DesafioBMG.Models;

namespace DesafioBMG.Repositories
{
    public interface IUserRepository
    {
        User? GetByEmailAndPassword(string email, string password);
    }
}
