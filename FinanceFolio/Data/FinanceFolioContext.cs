using FinanceFolio.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceFolio.Data;

public class FinanceFolioContext : DbContext
{
    public FinanceFolioContext(DbContextOptions<FinanceFolioContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
}