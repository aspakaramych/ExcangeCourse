using ExchangeCourse.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExchangeCourse.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<CurrencyEntity> Currencies { get; set; }
    public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrencyEntity>()
            .HasIndex(c => c.Code)
            .IsUnique();

        modelBuilder.Entity<ExchangeRateEntity>()
            .HasOne<CurrencyEntity>()
            .WithOne()
            .HasForeignKey<ExchangeRateEntity>(e => e.BaseCurrency);
        modelBuilder.Entity<ExchangeRateEntity>()
            .HasOne<CurrencyEntity>()
            .WithOne()
            .HasForeignKey<ExchangeRateEntity>(e => e.TargetCurrency);
            
        
        base.OnModelCreating(modelBuilder);
    }
}