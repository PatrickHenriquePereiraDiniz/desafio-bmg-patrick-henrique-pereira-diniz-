using DesafioBMG.Data;
using DesafioBMG.Models;

namespace DesafioBMG.Repositories
{
    public class EfUserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        public EfUserRepository(AppDbContext db) => _db = db;

        public User? GetByEmailAndPassword(string email, string password)
            => _db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
    }
}
