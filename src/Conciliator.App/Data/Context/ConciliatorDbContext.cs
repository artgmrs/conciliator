using Conciliator.App.Data.Mappings;
using Conciliator.App.Models;
using Microsoft.EntityFrameworkCore;

namespace Conciliator.App.Data.Context
{
    public class ConciliatorDbContext : DbContext
    {
        public ConciliatorDbContext(DbContextOptions options) : base (options)
        {
        }

        public DbSet<Extrato> Extratos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ExtratoMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}