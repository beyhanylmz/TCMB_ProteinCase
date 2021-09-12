using Microsoft.EntityFrameworkCore;
using ProteinCase.Entities;

namespace ProteinCase.Infrastructure
{
    public class CurrencyDbContext : DbContext
    {
        public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : base(options)
        {
        }

        public DbSet<Currency> Currencies { get; set; }
    }
}