using Microsoft.EntityFrameworkCore;
using CurrencyConverter.Models;

namespace CurrencyConverter.Data
{
    public class ApplicationDbContext : DbContext
    {
        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRate> CurrenciesRates {get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./CurrencyConverter.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<CurrencyRate>().HasKey(o => new {o.CurrencyId, o.Date});

            // builder.Entity<CurrencySerie>().HasOne(o => o.Currency)
            //                                .WithMany(b => b.Serie)
            //                                .HasForeignKey(p => p.CurrencyID)
            //                                .HasConstraintName("ForeingKey_CurrencySerie_Currency");
        }
    }
}