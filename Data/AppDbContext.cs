using DesafioBMG.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioBMG.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
}
