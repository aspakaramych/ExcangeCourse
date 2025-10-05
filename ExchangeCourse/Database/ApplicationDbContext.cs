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
            .HasOne(e => e.BaseCurrency)
            .WithMany()
            .HasForeignKey(e => e.BaseCurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ExchangeRateEntity>()
            .HasOne(e => e.TargetCurrency)
            .WithMany()
            .HasForeignKey(e => e.TargetCurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ExchangeRateEntity>()
            .HasIndex(e => new { e.BaseCurrencyId, e.TargetCurrencyId })
            .IsUnique();


        base.OnModelCreating(modelBuilder);
    }
}