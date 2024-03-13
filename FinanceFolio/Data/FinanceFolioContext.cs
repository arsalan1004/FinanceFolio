using FinanceFolio.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceFolio.Data;

public class FinanceFolioContext : DbContext
{
    public FinanceFolioContext(DbContextOptions<FinanceFolioContext> options) : base(options)
    {
    }

    private DbSet<User> Users { get; set; }
}