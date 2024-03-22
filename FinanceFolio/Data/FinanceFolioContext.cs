using FinanceFolio.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceFolio.Data;

public class FinanceFolioContext : DbContext
{
    public FinanceFolioContext(DbContextOptions<FinanceFolioContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<Account> Account { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Entry> Entry { get; set; }
}